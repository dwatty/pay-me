using Orleans;
using PayMe.Enums;
using PayMe.Models;

namespace PayMe.Grains
{
    public interface IGameGrain : IGrainWithGuidKey
    {
        Task<GameState> AddPlayerToGame(Guid player);
        Task<GameState> GetState();
        Task<GameSummary> GetSummary(Guid player);
        Task SetName(string name);
        Task<Card> DrawCard(Guid player);
        Task<object> TakeDiscard(Guid player);
        Task DiscardCard(Guid player, Suites suite, int value);
        Task EndTurn(Guid player);



        Task<GameState> MakeMove(GameMove move);
        Task<List<GameMove>> GetMoves();

    }



    [Serializable]
    public struct GameMove
    {
        public Guid PlayerId { get; set; }
        public GameAction Action { get; set; }
        public Card? Card { get; set; }
    }

    [Serializable]
    public struct GameSummary
    {
        public Guid GameId { get; set; }
        public GameState State { get; set; }
        public string Name { get; set; }
        public int NumPlayers { get; set; }
        public Card? LastDiscard { get; set; }
        public List<Card> Hand { get; set; }
        public bool YourMove { get; set; }
        public GameRound Round { get; set; }        
        public TurnState PlayerTurnState { get; set; }




        //
        // TBD
        
        public int NumMoves { get; set; }
        public GameOutcome Outcome { get; set; }
        public string[] Usernames { get; set; }
        
    }
}