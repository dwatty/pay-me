using Orleans;
using PayMe.Enums;
using PayMe.Models;

namespace PayMe.Grains
{
    public interface IGameGrain : IGrainWithGuidKey
    {
        Task<GameState> AddPlayerToGame(Guid player);
        Task<GameState> GetState();
        Task<List<GameMove>> GetMoves();
        Task<GameState> MakeMove(GameMove move);
        Task<GameSummary> GetSummary(Guid player);
        Task SetName(string name);

        Task<Card> DrawCard(Guid player);
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

        public Card LastDiscard { get; set; }

        public List<Card> Hand { get; set; }
        public bool YourMove { get; set; }
        


        //
        // TBD
        
        public int NumMoves { get; set; }
        public GameOutcome Outcome { get; set; }
        public string[] Usernames { get; set; }
        
    }
}