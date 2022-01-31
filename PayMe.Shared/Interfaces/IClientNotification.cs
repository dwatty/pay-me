using PayMe.Shared.Enums;
using PayMe.Shared.Infrastructure;
using PayMe.Shared.Models;

namespace PayMe.Shared.Interfaces;

public interface IClientNotification
{
    Task GameCreated();
    Task GameStarted(Stack<Card> discard, List<Card> availableCards);
    Task NewDiscardAvailable(Card nextDiscard);
    Task CardDiscarded(Card discarded);
    Task EndRound(Dictionary<GameRound, List<RoundResult>> results);
    Task EndTurn(Guid nextPlayerId);
    Task RoundWon(string playerName);
}