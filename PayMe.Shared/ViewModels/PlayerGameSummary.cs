using PayMe.Shared.Enums;
using PayMe.Shared.Models;

namespace PayMe.Shared.ViewModels;

public class PlayerGameSummary
{
    public Guid GameId { get; set; }
    public Guid GameOwner { get; set; }    
    public GameState State { get; set; }    
    public string Name { get; set; } = "";
    public int NumPlayers { get; set; }
    public Card? LastDiscard { get; set; }
    public GameRound Round { get; set; }        
    public RoundState RoundState { get; set; }
    public TurnState PlayerTurnState { get; set; }
    public Dictionary<GameRound, List<RoundResult>> Scoreboard { get; set; }= new Dictionary<GameRound, List<RoundResult>>();
    public List<Card> Hand { get; set; } = new List<Card>();
    public bool YourMove { get; set; }

    public List<Player> Players { get; set; } = new List<Player>();
    
}

public class Player
{
    public Guid PlayerId { get; set; }
    public string PlayerName { get; set; } = "";
}