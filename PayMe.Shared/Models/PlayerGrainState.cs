namespace Payme.Shared;

[Serializable]
public class PlayerGrainState
{
    public string Username { get; set; } = "";
    public List<Guid> ActiveGames = new List<Guid>();
    public List<Guid> PastGames = new List<Guid>();
    public int GamesStarted;

}