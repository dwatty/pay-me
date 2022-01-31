using Orleans;
using PayMe.Shared.Enums;
using PayMe.Shared.Models;
using PayMe.Shared.ViewModels;

namespace PayMe.Shared.Interfaces;

public interface IPlayerGrain : IGrainWithGuidKey
{
    // get a list of all active games
    Task<PairingSummary[]> GetAvailableGames();
    Task<List<GameListViewModel>> GetGameSummaries();
    // create a new game and join it
    Task<Guid> CreateGame();
    // join an existing game
    Task<GameState> JoinGame(Guid gameId);
    Task LeaveGame(Guid gameId, GameOutcome outcome);
    Task SetUsername(string username);
    Task<string> GetUsername();
    Task<List<Guid>> GetActiveGames();
}
