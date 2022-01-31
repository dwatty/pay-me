using Orleans.Concurrency;

namespace PayMe.Shared.Models;

[Serializable]
public class PairingsState
{
    public List<PairingSummary> Pairings { get; set; } = new List<PairingSummary>();
}

[Immutable]
[Serializable]
public class PairingSummary
{
    public Guid GameId { get; set; }
    public string Name { get; set; } = "";
}