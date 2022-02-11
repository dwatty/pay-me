using PayMe.Shared.Enums;

namespace PayMe.Shared.Models;

[Serializable]
public struct GameEvent
{
    public Guid? PlayerId { get; set; }
    public GameEvents Event { get; set; }
    public string EventBody { get; set; }
    public DateTimeOffset EventTime { get; set; }
}

[Serializable]
public class GameGrainState
{
    public Guid GameId { get; set; }
    public List<Guid> PlayerIds = new List<Guid>();

    public Guid GameOwner { get; set; }    
    public GameState State { get; set; }    
    public string Name { get; set; } = "";
    public int NumPlayers { get; set; }
    public Card? LastDiscard { get; set; }
    public GameRound Round { get; set; }        
    public RoundState RoundState { get; set; }
    public TurnState PlayerTurnState { get; set; }
    public Dictionary<GameRound, List<RoundResult>> Scoreboard { get; set; }= new Dictionary<GameRound, List<RoundResult>>();
    public int IndexNextPlayerToStart { get; set; }
    public int IndexNextPlayerToMove { get; set; }
    public List<Card> AvailableCards { get; set; } = new List<Card>();
    public Stack<Card> DiscardPile { get; set; } = new Stack<Card>();
    public Dictionary<Guid, List<Card>> AllHands = new Dictionary<Guid, List<Card>>();


    public List<GameEvent> Events { get; set; } = new List<GameEvent>();
    
}