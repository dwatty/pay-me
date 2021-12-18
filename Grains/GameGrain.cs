using Microsoft.AspNetCore.SignalR;
using Orleans;
using Orleans.Concurrency;
using PayMe.Enums;
using PayMe.Hubs;
using PayMe.Models;

namespace PayMe.Grains
{
    /// <summary>
    /// Orleans grain implementation class GameGrain
    /// </summary>
    [Reentrant]
    public class GameGrain : Grain, IGameGrain
    {
        // DI
        private readonly IHubContext<GameHub> _gameHub;
        
        //
        // Properties

        // The players that are playing
        private List<Guid> _playerIds = new List<Guid>();
        // Whose turn is next
        private int indexNextPlayerToMove = 0;
        // The state of the current game
        public GameState _gameState = GameState.AwaitingPlayers;
        // List of Available Cards
        private Deck _availableCards = new Deck();
        // List of Cards in Discard Pile
        private Stack<Card> _discardPile = new Stack<Card>();
        // Dictionary of Player Hands (Cards by Player ID)
        private Dictionary<Guid, List<Card>> _hands = new Dictionary<Guid, List<Card>>();
        // The current round the game is on
        private GameRound _currentRound = GameRound.Twos;

        
        
        
        
        
        //
        // TBD
        public Guid _winnerId = Guid.Empty;
        public Guid _loserId = Guid.Empty;
        // we record a game in terms of each of the moves, so we could reconstruct the sequence of play
        // during an active game, we also use a 2D array to represent the board, to make it
        //  easier to check for legal moves, wining lines, etc. 
        //  -1 represents an empty square, 0 & 1 the player's index 
        public List<GameMove> _moves = new List<GameMove>();
        private int[,] _board = new int[0,0];
        private string _name = "";



        public GameGrain(IHubContext<GameHub> gameHubContext)
        {
            _gameHub = gameHubContext;
        }

        // initialise 
        public override Task OnActivateAsync()
        {
            // make sure newly formed game is in correct state 
            // _playerIds = new List<Guid>();
            // _moves = new List<GameMove>();
            // indexNextPlayerToMove = -1;  // safety default - is set when game begins to 0 or 1
            // _board = new int[3, 3] { { -1, -1, -1 }, { -1, -1, -1 }, { -1, -1, -1 } };  // -1 is empty

            // _gameState = GameState.AwaitingPlayers;
            // _winnerId = Guid.Empty;
            // _loserId = Guid.Empty;

            return base.OnActivateAsync();
        }

        // add a player into a game
        public async Task<GameState> AddPlayerToGame(Guid player)
        {
            // check if its ok to join this game
            if (_gameState == GameState.Finished)
            {
                throw new ApplicationException("Can't join game once its over");
            } 

            if (_gameState == GameState.InPlay)
            {
                throw new ApplicationException("Can't join game once its in play");
            }

            // Add player
            _playerIds.Add(player);
            if(!_hands.ContainsKey(player))
            {
                _hands.Add(player, new List<Card>());
            }

            // Check if the game is ready to play
            if (_gameState == GameState.AwaitingPlayers && _playerIds.Count == 2)
            {
                _availableCards.FillDeck();
                _availableCards.ShuffleDeck();
                
                // Deal Cards to players
                foreach (var pID in _playerIds)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        _hands[pID].Add(_availableCards.DrawCard());    
                    }
                }

                // Prime the discard pile with a single card
                _discardPile.Push(_availableCards.DrawCard());

                // a new game is starting
                _gameState = GameState.InPlay;
                indexNextPlayerToMove = new Random().Next(0, _playerIds.Count - 1);

                await _gameHub
                    .Clients
                    .All
                    .SendAsync("GameStarted", new {
                        DiscardPile = _discardPile,
                        AvailablePile = _availableCards
                    });
            }

            // let user know if game is ready or not
            return _gameState;
        }


        //
        // Draw the next available card
        public async Task<Card> DrawCard(Guid player)
        {
            var nextCard = _availableCards.DrawCard();
            _hands[player].Add(nextCard);
            return await Task.FromResult(nextCard);
        }


        // make a move during the game
        public async Task<GameState> MakeMove(GameMove move)
        {
            // check if its a legal move to make
            if (_gameState != GameState.InPlay) throw new ApplicationException("This game is not in play");

            if (_playerIds.IndexOf(move.PlayerId) < 0) throw new ArgumentException("No such playerid for this game", "move");
            if (move.PlayerId != _playerIds[indexNextPlayerToMove]) throw new ArgumentException("The wrong player tried to make a move", "move");

            // if (move.X < 0 || move.X > 2 || move.Y < 0 || move.Y > 2) throw new ArgumentException("Bad co-ordinates for a move", "move");
            // if (_board[move.X, move.Y] != -1) throw new ArgumentException("That square is not empty", "move");

            // record move
            _moves.Add(move);
           // _board[move.X, move.Y] = indexNextPlayerToMove;

            // check for a winning move
            var win = false;
            if (!win)
            {
                for (int i = 0; i < 3 && !win; i++)
                {
                    win = IsWinningLine(_board[i, 0], _board[i, 1], _board[i, 2]);
                }
            }

            if (!win)
            {
                for (int i = 0; i < 3 && !win; i++)
                {
                    win = IsWinningLine(_board[0, i], _board[1, i], _board[2, i]);
                }
            }

            if (!win)
            {
                win = IsWinningLine(_board[0, 0], _board[1, 1], _board[2, 2]);
            }

            if (!win)
            {
                win = IsWinningLine(_board[0, 2], _board[1, 1], _board[2, 0]);
            }

            // check for draw
            var draw = false;
            if (_moves.Count() == 9)
            {
                draw = true;  // we could try to look for stalemate earlier, if we wanted 
            }

            // handle end of game
            if (win || draw)
            {
                // game over
                _gameState = GameState.Finished;
                if (win)
                {
                    _winnerId = _playerIds[indexNextPlayerToMove];
                    _loserId = _playerIds[(indexNextPlayerToMove + 1) % 2];
                }

                // collect tasks up, so we await both notifications at the same time
                var promises = new List<Task>();
                // inform this player of outcome
                var playerGrain = GrainFactory.GetGrain<IPlayerGrain>(_playerIds[indexNextPlayerToMove]);
                //promises.Add(playerGrain.LeaveGame(this.GetPrimaryKey(), win ? GameOutcome.Win : GameOutcome.Draw));

                // inform other player of outcome
                playerGrain = GrainFactory.GetGrain<IPlayerGrain>(_playerIds[(indexNextPlayerToMove + 1) % 2]);
                //promises.Add(playerGrain.LeaveGame(this.GetPrimaryKey(), win ? GameOutcome.Lose : GameOutcome.Draw));
                await Task.WhenAll(promises);
                return _gameState;
            }

            // if game hasnt ended, prepare for next players move
            indexNextPlayerToMove = (indexNextPlayerToMove + 1) % 2;
            return _gameState;
        }

        private bool IsWinningLine(int i, int j, int k) => (i, j, k) switch
        {
            (0, 0, 0) => true,
            (1, 1, 1) => true,
            _ => false
        };

        public Task<GameState> GetState() => Task.FromResult(_gameState);

        public Task<List<GameMove>> GetMoves() => Task.FromResult(_moves);

        public async Task<GameSummary> GetSummary(Guid player)
        {
            var promises = new List<Task<string>>();
            foreach (var p in _playerIds.Where(p => p != player))
            {
                promises.Add(GrainFactory.GetGrain<IPlayerGrain>(p).GetUsername());
            }

            await Task.WhenAll(promises);

            return new GameSummary
            {
                NumMoves = _moves.Count,
                State = _gameState,
                YourMove = _gameState == GameState.InPlay && player == _playerIds[indexNextPlayerToMove],
                NumPlayers = _playerIds.Count,
                GameId = this.GetPrimaryKey(),
                Usernames = promises.Select(x => x.Result).ToArray(),
                Name = _name,
                LastDiscard = _discardPile.Peek(),
                Hand = _hands[player]
            };
        }

        public Task SetName(string name)
        {
            _name = name;
            return Task.CompletedTask;
        }
    }
}