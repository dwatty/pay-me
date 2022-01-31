using Orleans;
using Orleans.Concurrency;
using Orleans.Runtime;
using PayMe.Shared;
using PayMe.Shared.Interfaces;
using PayMe.Shared.Models;

namespace PayMe.Grains;

[Reentrant]
public class PairingGrain : Grain, IPairingGrain
{
    private readonly IPersistentState<PairingsState> _pairings;

    public PairingGrain([PersistentState("pairing", Constants.PAIRING_GRAIN_STORAGE_NAME)] IPersistentState<PairingsState> pairings)
    {
        _pairings = pairings;
    }

    public async Task AddGame(Guid gameId, string name)
    {
        _pairings.State.Pairings.Add(new PairingSummary
        {
            GameId = gameId,
            Name = name
        });

        await _pairings.WriteStateAsync();
    }

    public async Task RemoveGame(Guid gameId)
    {
        var match = _pairings.State.Pairings.FirstOrDefault(x => x.GameId == gameId);
        if(match != null)
        {
            _pairings.State.Pairings.Remove(match);
        }
    
        await _pairings.WriteStateAsync();
    }

    public Task<PairingSummary[]> GetGames() => Task.FromResult(
        _pairings.State.Pairings.ToArray()
    );

}