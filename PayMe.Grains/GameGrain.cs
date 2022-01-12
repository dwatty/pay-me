using Orleans;
using Orleans.Concurrency;
using PayMe.Shared.Enums;
using PayMe.Shared.Infrastructure;
using PayMe.Shared.Models;
using PayMe.Shared.Interfaces;
using Orleans.Runtime;
using PayMe.Shared;
using PayMe.Shared.ViewModels;

namespace PayMe.Grains;

/// <summary>
/// Orleans grain implementation class GameGrain
/// </summary>
[Reentrant]
public class GameGrain : Grain, IGameGrain
{
    //
    // DI
    private readonly IClientNotification _clientNotifcation;
    private readonly IPersistentState<GameGrainState> _game;
    private IDeck _deckService;

    //
    // Ctor
    public GameGrain(
        IClientNotification gameHubContext, 
        [PersistentState("game", Constants.GAME_GRAIN_STORAGE_NAME)] IPersistentState<GameGrainState> game,
        IDeck deckService)
    {
        _clientNotifcation = gameHubContext;
        _game = game;
        _deckService = deckService;
    }

    //
    // Initialize the Game grain 
    public override Task OnActivateAsync()
    {
        if(_game.State.Name is null)
        {
            // Is there anything we need to do for a new game?
        }

        return base.OnActivateAsync();
    }

    // 
    // Add the provided player to the game
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

        // Add player
        _game.State.PlayerIds.Add(player);
        if(!_game.State.AllHands.ContainsKey(player))
        {
            _game.State.AllHands.Add(player, new List<Card>());
        }

        // Check if the game is ready to play
        if(_game.State.State == GameState.AwaitingPlayers && _game.State.PlayerIds.Count == 2)
        {
            await SetupRound(GameRound.Threes);
        }

        await _game.WriteStateAsync();
        return _game.State.State;
    }

    //
    // Ready the game for the provided round
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
        _game.State.IndexNextPlayerToMove = new Random().Next(0, _game.State.PlayerIds.Count - 1);
        
        await _game.WriteStateAsync();
        await _clientNotifcation.GameStarted(_game.State.DiscardPile, _game.State.AvailableCards);
    }

    //
    // Draw the next available card
    public async Task<Card> DrawCard(Guid player)
    {
        var nextCard = DrawCard();
        _game.State.AllHands[player].Add(nextCard);
        _game.State.PlayerTurnState = TurnState.DrewCard;

        await _game.WriteStateAsync();
        return await Task.FromResult(nextCard);
    }

    // 
    // Take the top discard and assign it to the player
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

        await _clientNotifcation.NewDiscardAvailable(nextCard);
        await _game.WriteStateAsync();

        return await Task.FromResult(new {
            NextCard = nextCard,
            NextDiscard = nextDiscard
        });            
    }

    //
    // Get and return the summary of the game
    public async Task<PlayerGameSummary> GetPlayerSummary(Guid player)
    {
        var promises = new List<Task<string>>();
        foreach (var p in _game.State.PlayerIds.Where(p => p != player))
        {
            promises.Add(GrainFactory.GetGrain<IPlayerGrain>(p).GetUsername());
        }

        await Task.WhenAll(promises);

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
        };

        return result;
    }

    //
    // Set the friendly name for this game
    public async Task SetName(string name)
    {
        _game.State.Name = name;
        await _game.WriteStateAsync();
    }

    //
    // Add a card from the players hand into the discard pile
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

            await _clientNotifcation.CardDiscarded(discarded);
        }
        else
        {
            throw new Exception("Discard request not found in user's hand.");
        }

        await _game.WriteStateAsync();
    }

    //
    // End the player's turns
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
                _game.State.RoundState = RoundState.Finished;
                await _clientNotifcation.EndRound(_game.State.Scoreboard);
            }
        }
        
        // Notify clients that the turn is over and who is up next
        await _clientNotifcation.EndTurn(nextPlayerId);
        await _game.WriteStateAsync();
    }

    //
    // Claim a win
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
                
                _game.State.Scoreboard.Add(_game.State.Round, roundResults);
            }
            else
            {
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
            await _game.WriteStateAsync();
            return ClaimResult.Invalid;
        }
    }

    //
    // Reset things and advance to the next round
    public async Task StartNextRound()
    {
        await SetupRound(_game.State.Round + 1);
    }


    public bool CanMove(Guid playerId)
    {
        return playerId ==  _game.State.PlayerIds[_game.State.IndexNextPlayerToMove];
    }

    public Task<GameListViewModel> GetSummary(Guid gameId)
    {
        var result = new GameListViewModel
        {
            NumPlayers = _game.State.PlayerIds.Count,
            Name = _game.State.Name,
            GameId = this.GetPrimaryKey()
        };
        
        return Task.FromResult(result);
    }

    private Card DrawCard()
    {
        var nextCard = _game.State.AvailableCards.ElementAt(0);
        _game.State.AvailableCards.RemoveAt(0);
        return nextCard;
    }

}