using Orleans;
using PayMe.Shared.Enums;
using PayMe.Shared.Models;
using PayMe.Shared.ViewModels;

namespace PayMe.Shared.Interfaces;

public interface IGameGrain : IGrainWithGuidKey
{
    Task<GameState> AddPlayerToGame(Guid player);
    Task<PlayerGameSummary> GetPlayerSummary(Guid player);
    Task SetName(string name);
    Task<Card> DrawCard(Guid player);
    Task<object> TakeDiscard(Guid player);
    Task DiscardCard(Guid player, Suites suite, int value);
    Task EndTurn(Guid player, List<List<Card>> groups, bool winningTurn);
    Task<ClaimResult> ClaimWin(Guid player, List<List<Card>> groups);
    Task StartNextRound();


    // Non-Game Actions
    Task<GameListViewModel> GetSummary();
    Task<List<GameEvent>> GetGameHistory();
}