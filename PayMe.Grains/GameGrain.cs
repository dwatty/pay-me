using Orleans;
using Orleans.Concurrency;
using PayMe.Shared.Enums;
using PayMe.Shared.Infrastructure;
using PayMe.Shared.Models;
using PayMe.Shared.Interfaces;
using Orleans.Runtime;
using PayMe.Shared;
using PayMe.Shared.ViewModels;
using Newtonsoft.Json;
using System.Dynamic;

namespace PayMe.Grains;

/// <summary>
/// Orleans grain implementation class GameGrain
/// </summary>
[Reentrant]
public class GameGrain : Grain, IGameGrain
{
    private readonly IClientNotification _clientNotifcation;
    private readonly IPersistentState<GameGrainState> _game;
    private IDeck _deckService;

    /// <summary>
    /// Our GameGrain constructor
    /// </summary>
    public GameGrain(
        IClientNotification gameHubContext, 
        [PersistentState("game", Constants.GAME_GRAIN_STORAGE_NAME)] IPersistentState<GameGrainState> game,
        IDeck deckService)
    {
        _clientNotifcation = gameHubContext;
        _game = game;
        _deckService = deckService;
    }

    /// <summary>
    /// Adds the provided player ID to the current game.
    /// Will also start the current game when there are
    /// enough players
    /// </summary>
    /// <param name="player">The Guid of the player to add to the game</param>
    public async Task<GameState> AddPlayerToGame(Guid player)
    {
        // check if its ok to join this game
        if (_game.State.State == GameState.Finished)
        {
            throw new ApplicationException("Can't join game once its over");
        }

        if (_game.State.State == GameState.InPlay)
        {
            throw new ApplicationException("Can't join game once its in play");
        }

        AddPlayer(player);

        // Check if the game is ready to play
        if (_game.State.State == GameState.AwaitingPlayers && _game.State.PlayerIds.Count == 2)
        {
            AddEvent(GameEvents.GameStarted, "Game Starting");
            await SetupRound(GameRound.Threes);
        }

        await _game.WriteStateAsync();
        return _game.State.State;
    }

    /// <summary>
    /// Draw a card from the deck and add it to the players
    /// hand.
    /// </summary>
    /// <param name="player">The Guid of the player to draw for</param>
    public async Task<Card> DrawCard(Guid player)
    {
        var nextCard = DrawCard();
        _game.State.AllHands[player].Add(nextCard);
        _game.State.PlayerTurnState = TurnState.DrewCard;

        AddCardEvent(player, GameEvents.DrawCard, nextCard);

        await _game.WriteStateAsync();
        return await Task.FromResult(nextCard);
    }

    /// <summary>
    /// Draw a card from the discard pile and add it to the players
    /// hand.
    /// </summary>
    /// <param name="player">The Guid of the player to draw for</param>
    public async Task<object> TakeDiscard(Guid player)
    {
        if(_game.State.DiscardPile.Count == 0) 
        {
            throw new Exception("No discards to take");
        }

        var nextCard = _game.State.DiscardPile.Pop();
        _game.State.AllHands[player].Add(nextCard);
        _game.State.PlayerTurnState = TurnState.DrewCard;

        Card? nextDiscard;
        _game.State.DiscardPile.TryPeek(out nextDiscard);

        AddCardEvent(player, GameEvents.DrawDiscard, nextDiscard);

        await _clientNotifcation.NewDiscardAvailable(nextCard);
        await _game.WriteStateAsync();

        return await Task.FromResult(new {
            NextCard = nextCard,
            NextDiscard = nextDiscard
        });            
    }

    /// <summary>
    /// Get and return the summary of the game from the perspective of
    /// the provided player ID.
    /// </summary>
    /// <param name="player">The Guid of the player to get a summary for</param>
    public async Task<PlayerGameSummary> GetPlayerSummary(Guid player)
    {
        var players = new List<Player>();
        foreach(var p in _game.State.PlayerIds)
        {
            var username = await GrainFactory.GetGrain<IPlayerGrain>(p).GetUsername();

            players.Add(new Player()
            {
                PlayerId = p,
                PlayerName = username
            });
        }

        var result = new PlayerGameSummary()
        {
            State = _game.State.State,
            YourMove = _game.State.State == GameState.InPlay && player == _game.State.PlayerIds[_game.State.IndexNextPlayerToMove],
            NumPlayers = _game.State.PlayerIds.Count,
            GameId = this.GetPrimaryKey(),
            Name = _game.State.Name,
            LastDiscard = _game.State.DiscardPile.Count > 0 ? _game.State.DiscardPile.Peek() : null,
            Hand = _game.State.AllHands[player],
            Round = _game.State.Round,
            RoundState = _game.State.RoundState,
            PlayerTurnState = _game.State.PlayerTurnState,
            GameOwner = _game.State.PlayerIds[0],
            Scoreboard = _game.State.Scoreboard,
            Players = players
        };

        AddEvent(GameEvents.GetSumamry, result);

        return result;
    }

    /// <summary>
    /// Set the friendly name for this game
    /// </summary>
    /// <param name="name">The name to use for the game</param>
    public async Task SetName(string name)
    {
        _game.State.Name = name;
        AddEvent(GameEvents.SetName, name);
        await _game.WriteStateAsync();
    }

    /// <summary>
    /// Add a card from the players hand into the discard pile
    /// </summary>
    /// <param name="player">The player we're actioning</param>
    /// <param name="suite">The suite of the card from their hand</param>
    /// <param name="value">The value of the card from their hand</param>
    public async Task DiscardCard(Guid player, Suites suite, int value)
    {
        var foundIdx = -1;
        for (var i = 0; i < _game.State.AllHands[player].Count; i++)
        {
            var card = _game.State.AllHands[player][i];
            if(card.Suite == suite && card.Value == value)
            {
                foundIdx = i;
                break;
            }
        }

        if(foundIdx > -1)
        {
            var discarded = _game.State.AllHands[player].ElementAt(foundIdx);
            _game.State.DiscardPile.Push(discarded);
            _game.State.AllHands[player].RemoveAt(foundIdx);
            _game.State.PlayerTurnState = TurnState.Discarded;

            AddCardEvent(player, GameEvents.Discard, discarded);
            await _clientNotifcation.CardDiscarded(discarded);
        }
        else
        {
            throw new Exception("Discard request not found in user's hand.");
        }
        
        await _game.WriteStateAsync();
    }

    /// <summary>
    /// End the player's turn
    /// </summary>
    /// <param name="player">The player we're actioning</param>
    /// <param name="groups">The player's hand, grouped how they like</param>
    /// <param name="winningTurn">A boolean indicating if this turn was deemed a winner</param>
    public async Task EndTurn(Guid player, List<List<Card>> groups, bool winningTurn)
    {            
        if(!CanMove(player)) 
        { 
            return; 
        }

        _game.State.IndexNextPlayerToMove = (_game.State.IndexNextPlayerToMove + 1) % 2;        
        var nextPlayerId = _game.State.PlayerIds[_game.State.IndexNextPlayerToMove];
        _game.State.PlayerTurnState = TurnState.TurnStarted;

        // Someone has won this round already, so we need to
        // stop action after everyone else has had one last turn
        // We also need to score people's hands            
        if(!winningTurn && _game.State.Scoreboard.TryGetValue(_game.State.Round, out var currentRoundResult))
        {
            var validityResult = ValidityEngine.ValidateHand(groups, _game.State.Round);
            var score = ScoringEngine.ScoreHand(validityResult, _game.State.Round);

            AddScoreEvent(player, groups, validityResult, score);

            var playerGrain = GrainFactory.GetGrain<IPlayerGrain>(player);
            var playerName = await playerGrain.GetUsername();

            currentRoundResult.Add(new RoundResult
            {
                PlayerId = player,
                PlayerName = playerName,
                Score = score,
                WonRound = false
            });
            
            // The next player to go is the one that won this round
            // So consider this the end of the round and reset        
            if(nextPlayerId == currentRoundResult[0].PlayerId)
            {
                AddEvent(GameEvents.RoundOver, $"Round {_game.State.Round} has completed");
                
                _game.State.RoundState = RoundState.Finished;
                await _clientNotifcation.EndRound(_game.State.Scoreboard);
            }
        }
        
        // Notify clients that the turn is over and who is up next
        await _clientNotifcation.EndTurn(nextPlayerId);
        await _game.WriteStateAsync();
    }

    /// <summary>
    /// Check a user's win claim.  Called from the client when a user thinks
    /// they've gone out.
    /// </summary>
    /// <param name="player">The player we're actioning</param>
    /// <param name="groups">The player's hand, grouped how they like</param>
    public async Task<ClaimResult> ClaimWin(Guid player, List<List<Card>> groups)
    {
        var validityResult = ValidityEngine.ValidateHand(groups, _game.State.Round);
        if(validityResult.AllSetsValid())
        {
            var playerGrain = GrainFactory.GetGrain<IPlayerGrain>(player);
            var playerName = await playerGrain.GetUsername();

            // Note that this player wins this round.
            // Confirm that score is calculated as zero for sanity
            var score = ScoringEngine.ScoreHand(validityResult, _game.State.Round);
            if(score == 0)
            {
                var roundResults = new List<RoundResult>()
                {
                    new RoundResult() 
                    { 
                        PlayerId = player, 
                        Score = score,
                        PlayerName = playerName,
                        WonRound = true
                    }
                };
                
                AddClaimEvent(player, groups, score, true);
                _game.State.Scoreboard.Add(_game.State.Round, roundResults);
            }
            else
            {
                AddClaimEvent(player, groups, score, false);
                throw new Exception("Winning hand was found but score wasn't zero.");
            }
            
            // Notify all clients that this player won
            await _clientNotifcation.RoundWon(playerName);

            // End this players turn
            await EndTurn(player, groups, true);
            await _game.WriteStateAsync();

            return ClaimResult.Valid;
        }
        else
        {
            AddClaimEvent(player, groups, -1, false);
            await _game.WriteStateAsync();
            return ClaimResult.Invalid;
        }
    }

    /// <summary>
    /// Reset our information and start the next round
    /// </summary>
    public async Task StartNextRound()
    {
        await SetupRound(_game.State.Round + 1);
    }

    /// <summary>
    /// Return a condensed VM of the current grain
    /// </summary>
    public Task<GameListViewModel> GetSummary()
    {
        var result = new GameListViewModel
        {
            NumPlayers = _game.State.PlayerIds.Count,
            Name = _game.State.Name,
            GameId = this.GetPrimaryKey()
        };
        
        return Task.FromResult(result);
    }

    /// <summary>
    /// Return the history of game events
    /// </summary>
    public Task<List<GameEvent>> GetGameHistory()
    {
        var result = _game.State.Events
            .OrderBy(x => x.EventTime)
            .ToList();

        return Task.FromResult(result);
    }


    /// <summary>
    /// Helper method to draw a card from the available deck
    /// </summary>
    private Card DrawCard()
    {
        var nextCard = _game.State.AvailableCards.ElementAt(0);
        _game.State.AvailableCards.RemoveAt(0);
        return nextCard;
    }

    /// <summary>
    /// Helper to determine if the provided player is allowed
    /// to move or not
    /// </summary>
    /// <param name="player">The Guid of the player to get a summary for</param>
    private bool CanMove(Guid player)
    {
        return player ==  _game.State.PlayerIds[_game.State.IndexNextPlayerToMove];
    }

    /// <summary>
    /// Fill, shuffle, and deal cards from the deck to all
    /// players in the game based on the provided round.
    /// </summary>
    /// <param name="round">Which round we're dealing, influences how many cards</param>
    private async Task SetupRound(GameRound round)
    {
        // We use the deck service to generate a deck and shuffle it,
        // but then we copy that to the grain state and use that going
        // forward
        _deckService.FillDeck();
        _deckService.ShuffleDeck();
        _game.State.AvailableCards = _deckService.GetCards();

        // Deal Cards to players
        foreach (var pID in _game.State.PlayerIds)
        {
            _game.State.AllHands[pID].Clear();

            for (int i = 0; i <  (int)round; i++)
            {
                _game.State.AllHands[pID].Add(DrawCard());
            }
        }

        // Prime the discard pile with a single card
        _game.State.DiscardPile.Push(DrawCard());

        // a new game is starting
        _game.State.Round = round;
        _game.State.State = GameState.InPlay;
        _game.State.PlayerTurnState = TurnState.TurnStarted;
        _game.State.RoundState = RoundState.InPlay;
        
        // If this is the first round, initialize who starts
        if(round == GameRound.Threes)
        {
            _game.State.IndexNextPlayerToMove = new Random().Next(0, _game.State.PlayerIds.Count - 1);
            _game.State.IndexNextPlayerToStart = _game.State.IndexNextPlayerToMove;
        }
        else 
        {
            _game.State.IndexNextPlayerToStart = (_game.State.IndexNextPlayerToStart + 1) % 2;
            _game.State.IndexNextPlayerToMove = _game.State.IndexNextPlayerToStart;
        }
        
        AddEvent(GameEvents.HandDealt, $"Round {round.ToString()} dealt");
        
        await _game.WriteStateAsync();
        await _clientNotifcation.GameStarted(_game.State.DiscardPile, _game.State.AvailableCards);
    }

    /// <summary>
    /// Add the player to the game
    /// </summary>
    /// <param name="player">The Guid of the player to add to the game</param>
    private void AddPlayer(Guid player)
    {
        _game.State.PlayerIds.Add(player);
        AddEvent(GameEvents.PlayerAdded, player.ToString());

        if (!_game.State.AllHands.ContainsKey(player))
        {
            _game.State.AllHands.Add(player, new List<Card>());
        }
    }

    /// <summary>
    /// Add a game event relating to scoring
    /// </summary>
    /// <param name="player">The Guid of the player</param>
    /// <param name="groups">The player's card group</param>
    /// <param name="result">The validity result</param>
    /// <param name="score">The calcualted score</param>
    private void AddScoreEvent(Guid player, List<List<Card>> groups, ValidityResult result, int score)
    {
        dynamic evtBody = new ExpandoObject();
        evtBody.PlayerId = player;
        evtBody.Groups = groups;
        evtBody.Result = result;
        evtBody.Score = score;

        var scoreEvent = new GameEvent()
        {
            PlayerId = player,
            Event = GameEvents.ScoreResult,
            EventBody = JsonConvert.SerializeObject(evtBody),
            EventTime = DateTimeOffset.UtcNow
        };        

        _game.State.Events.Add(scoreEvent);
    }

    /// <summary>
    /// Add a game event relating to cards
    /// </summary>
    /// <param name="player">The Guid of the player</param>
    /// <param name="evt">The kind of event</param>
    /// <param name="card">The card being actioned</param>
    private void AddCardEvent(Guid player, GameEvents evt, Card card)
    {
        dynamic evtBody = new ExpandoObject();
        evtBody.PlayerId = player;
        evtBody.Suite = card != null ? card.Suite : Suites.Unknown;
        evtBody.Value = card != null ? card.Value : 0;

        var scoreEvent = new GameEvent()
        {
            PlayerId = player,
            Event = evt,
            EventBody = JsonConvert.SerializeObject(evtBody),
            EventTime = DateTimeOffset.UtcNow
        };        

        _game.State.Events.Add(scoreEvent);
    }

    /// <summary>
    /// Add a game event relating to cards
    /// </summary>
    /// <param name="player">The Guid of the player</param>
    /// <param name="groups">The player's card group</param>
    /// <param name="score">The calculated score</param>
    /// <param name="claimSuccess">Whether the claim was valid or not</param>
    private void AddClaimEvent(Guid player, List<List<Card>> groups, int score, bool claimSuccess) 
    {
        dynamic evtBody = new ExpandoObject();
        evtBody.PlayerId = player;
        evtBody.Groups = groups;
        evtBody.Score = score;

        var scoreEvent = new GameEvent()
        {
            PlayerId = player,
            Event = claimSuccess ? GameEvents.ClaimWin : GameEvents.ClaimFail,
            EventBody = JsonConvert.SerializeObject(evtBody),
            EventTime = DateTimeOffset.UtcNow
        };        

        _game.State.Events.Add(scoreEvent);
    }

    /// <summary>
    /// Add a general game event
    /// </summary>
    /// <param name="evt">The game event</param>
    /// <param name="body">The message to log</param>
    private void AddEvent(GameEvents evt, string message)
    {
        dynamic evtBody = new ExpandoObject();
        evtBody.Message = message;

        var scoreEvent = new GameEvent()
        {
            Event = evt,
            EventBody = JsonConvert.SerializeObject(evtBody),
            EventTime = DateTimeOffset.UtcNow
        };        

        _game.State.Events.Add(scoreEvent);
    }

    /// <summary>
    /// Add a general game event
    /// </summary>
    /// <param name="evt">The game event</param>
    /// <param name="body">The message to log</param>
    private void AddEvent(GameEvents evt, object body)
    {
        dynamic evtBody = new ExpandoObject();
        evtBody.Message = body;

        var scoreEvent = new GameEvent()
        {
            Event = evt,
            EventBody = JsonConvert.SerializeObject(evtBody),
            EventTime = DateTimeOffset.UtcNow
        };        

        _game.State.Events.Add(scoreEvent);
    }

}