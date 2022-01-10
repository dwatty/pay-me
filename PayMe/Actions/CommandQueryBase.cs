namespace PayMe.Commands;

public abstract class CommandQueryBase
{
    public Guid PlayerId { get; set; }
    public Guid GameId { get; set; }
    
}