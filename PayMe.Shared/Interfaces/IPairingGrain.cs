using Orleans;
using PayMe.Shared.Models;

namespace PayMe.Shared.Interfaces;

public interface IPairingGrain : IGrainWithIntegerKey
{
    Task AddGame(Guid gameId, string name);
    Task RemoveGame(Guid gameId);
    Task<PairingSummary[]> GetGames();
}