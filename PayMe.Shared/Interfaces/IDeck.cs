using PayMe.Shared.Models;

namespace PayMe.Shared.Interfaces;

public interface IDeck 
{
    void FillDeck();
    void ShuffleDeck();
    List<Card> GetCards();
}