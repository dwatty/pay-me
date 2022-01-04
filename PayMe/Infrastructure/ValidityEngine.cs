using PayMe.Enums;
using PayMe.Models;

namespace PayMe.Infrastructure
{
    public static class ValidityEngine
    {
        public static ValidityResult ValidateHand(List<List<Card>> sets, GameRound round)
        {
            var validity = new ValidityResult();

            // The total number of cards in the hand must 
            // equal the round amount
            var cardCount = sets.Sum(x => x.Count);
            if (cardCount != (int)round)
            {
                validity.IsInvalidCollection = true;
            }

            foreach (var set in sets)
            {
                var setResult = new SetValidityResult(set);

                // If the card count isn't valid, that's an immediate fail
                if (set.Count < 3 || set.Count > (int)round)
                {
                    setResult.IsValid = false;
                    // var setResult = new SetValidityResult(set, false);
                    //validity.Results.Add(setResult);
                }
                // Matching X of a Kind
                else if (AssertMatchingFaces(set, round))
                {
                    setResult.IsValid = true;
                    // var setResult = new SetValidityResult(set, true);
                    // validity.Results.Add(setResult);
                    //continue;
                }
                // Matching a Run
                else if (AssertRun(set, round))
                {
                    setResult.IsValid = true;
                    // var setResult = new SetValidityResult(set, true);
                    // validity.Results.Add(setResult);
                    //continue;
                }
                // No run or X of a kind
                else
                {
                    setResult.IsValid = false;
                    // var setResult = new SetValidityResult(set, false);
                    // validity.Results.Add(setResult);    
                }
                
                // Add our set result to our overall result
                validity.Results.Add(setResult);
                
            }

            return validity;
        }

        //
        // Check the provided list of Cards for a valid set of matching faces.
        // e.g. 3D-3H-3S
        public static bool AssertMatchingFaces(List<Card> set, GameRound round)
        {
            var distinctCount = set
                .Where(x => x.Value != (int)round && x.Value != Constants.JOKER)
                .DistinctBy(x => x.Value)
                .Count();

            return distinctCount == 1 || distinctCount == 0;
        }

        // 
        // Check the provided list of Cards for a valid run of at least 3 cards
        // e.g. 5D-6D-7D-8D
        public static bool AssertRun(List<Card> set, GameRound round)
        {
            if (set.Count > (int)round)
            {
                return false;
            }

            // For it to be a valid run, all suites, except wilds
            // must match.  A basic check that this is true to skip
            // further processing
            var distinctCount = set
                .Where(x => x.Value != (int)round && x.Value != Constants.JOKER)
                .DistinctBy(x => x.Suite)
                .Count();

            if (distinctCount > 1)
            {
                return false;
            }

            // Determine how many wilds we have for filling
            // in gaps later
            // var wilds = grp
            //     .Where(x => x.Value == (int)round || x.Value == Constants.JOKER)
            //     .Count();

            var wildCards = set
                .Where(x => x.Value == (int)round || x.Value == Constants.JOKER)
                .Select(x => x)
                .ToList();

            var orderedList = set
                .Where(x => x.Value != (int)round && x.Value != Constants.JOKER)
                .OrderBy(x => x.Value)
                .ToList();

            // If the first card is a 2 or we have wilds, and the last is an Ace, 
            // we're actually playing the Ace, so we want to move that to the front
            if ((orderedList[0].Value == 2 || wildCards.Count > 0) && orderedList[orderedList.Count - 1].Value == Constants.ACE)
            {
                var aceCard = orderedList.Single(x => x.Value == Constants.ACE);
                orderedList.Remove(aceCard);
                orderedList.Insert(0, aceCard);
            }

            var logicalHand = new List<Card>();
            for (int i = 0; i < set.Count; i++)
            {
                // First time through, treat card 0
                // as the start of our run
                if (i == 0)
                {
                    logicalHand.Add(orderedList[0]);
                    orderedList.RemoveAt(0);
                    continue;
                }

                // A special case where the Ace is being played as low
                if (logicalHand.Count == 1 && logicalHand[logicalHand.Count - 1].Value == Constants.ACE)
                {
                    // If we're playing the Ace low, then the next card
                    // must be a 2 or a wild.  If it's a two, push that 
                    // onto our list
                    if (orderedList[0].Value == 2)
                    {
                        logicalHand.Add(orderedList[0]);
                        orderedList.RemoveAt(0);
                    }
                    // Or pull a wild
                    else if(wildCards.Count > 0)
                    {
                        var nextWild = wildCards.First();                    
                        wildCards.Remove(nextWild);

                        var prevSuite = logicalHand[logicalHand.Count - 1].Suite;
                        var wildcard = new WildCard(2, prevSuite, nextWild.Value, nextWild.Suite);
                        logicalHand.Add(wildcard);
                    }
                    // We needed a filler and we don't have it, auto-fail
                    else
                    {
                        return false;
                    }
                }
                // We have extra wild cards, which is fine
                else if (orderedList.Count == 0 && wildCards.Count > 0)
                {
                    return true;
                }
                // The next card in the list is correctly sequential
                else if (logicalHand[logicalHand.Count - 1].Value + 1 == orderedList[0].Value)
                {
                    logicalHand.Add(orderedList[0]);
                    orderedList.RemoveAt(0);
                }
                // We need to use a wild card for this slot
                else if (wildCards.Count > 0)
                {
                    var nextWild = wildCards.First();                    
                    wildCards.Remove(nextWild);

                    var prevValue = logicalHand[logicalHand.Count - 1].Value;
                    var prevSuite = logicalHand[logicalHand.Count - 1].Suite;

                    var wildcard = new WildCard(prevValue + 1, prevSuite, nextWild.Value, nextWild.Suite);
                    logicalHand.Add(wildcard);
                }
                // We needed a wild but didn't have it, auto-fail.
                else
                {
                    return false;
                }

            }

            // How a basic run check works.
            // Uncomment for debugging the logicalHand
            // for (int i = 0; i < logicalHand.Count-1; i++)
            // {
            //     if (logicalHand[i].Value + 1 != logicalHand[i + 1].Value)
            //     {
            //         return false;
            //     }
            // }

            return true;

            // var wildsUsed = 0;
            // for (int i = 0; i < orderedList.Count() - 1; i++)
            // {
            //     // The Ace is being played low so we check if the next card is a 2
            //     // If it is, then the Ace is in a valid slot and we continue
            //     if (i == 0 && orderedList[i].Value == Constants.ACE && orderedList[i + 1].Value == 2)
            //     {
            //         continue;
            //     }
            //     else if (i == 0 && orderedList[i].Value == Constants.ACE && wilds > 0)
            //     {
            //         wildsUsed++;
            //         continue;
            //     }

            //     if (orderedList[i].Value + 1 != orderedList[i + 1].Value)
            //     {
            //         if (wildsUsed != wilds)
            //         {
            //             wildsUsed++;
            //         }
            //         else
            //         {
            //             return false;
            //         }
            //     }
            // }

            // return true;
        }

    }
}