using PayMe.Shared.Enums;
using PayMe.Shared.Models;

namespace PayMe.Shared.Interfaces;

public interface IGameHub
{
    Task JoinGame(Guid gameId);
    Task GameCreated();
    Task GameStarted(Stack<Card> discard, List<Card> availableCards);
    Task NewDiscardAvailable(Card nextDiscard);
    Task CardDiscarded(Card discarded);
    Task EndRound(Dictionary<GameRound, List<RoundResult>> results);
    Task EndTurn(Guid nextPlayerId);
    Task RoundWon(string playerName);

    Task PlayerCreated();
}