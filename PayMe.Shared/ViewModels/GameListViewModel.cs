namespace PayMe.Shared.ViewModels;

public class GameListViewModel 
{
    public Guid GameId { get; set; }
    public string Name { get; set; } = "";
    public int NumPlayers { get; set; }
}