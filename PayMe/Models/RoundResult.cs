namespace PayMe.Models;

public class RoundResult
{
    public Guid PlayerId { get; set; }
    public string PlayerName { get; set; } = "";
    public int Score { get; set; } = 0;
    public Boolean WonRound { get; set; } = false;

}