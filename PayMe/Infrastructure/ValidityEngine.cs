using PayMe.Enums;
using PayMe.Models;

namespace PayMe.Infrastructure
{
    public static class ValidityEngine
    {
        public static ClaimResult ClaimWin(List<List<Card>> groups, GameRound round)
        {
            // The total number of cards in the hand must 
            // equal the round amount
            var cardCount = groups.Sum(x => x.Count);
            if (cardCount != (int)round)
            {
                return ClaimResult.Invalid;
            }

            var validGroups = 0;
            foreach (var grp in groups)
            {
                if (grp.Count < 3 || grp.Count > (int)round)
                {
                    return ClaimResult.Invalid;
                }

                // Matching X of a Kind
                if (AssertMatchingFaces(grp, round))
                {
                    validGroups++;
                    continue;
                }

                // Matching a Run
                if (AssertRun(grp, round))
                {
                    validGroups++;
                    continue;
                }
            }

            return (validGroups == groups.Count) ? ClaimResult.Valid : ClaimResult.Invalid;            
        }


        public static bool AssertMatchingFaces(List<Card> grp, GameRound round)
        {
            var distinctCount = grp
                .Where(x => x.Value != (int)round && x.Value != Constants.JOKER)
                .DistinctBy(x => x.Value)
                .Count();

            return distinctCount == 1 || distinctCount == 0;
        }

        public static bool AssertRun(List<Card> grp, GameRound round)
        {
            if (grp.Count > (int)round)
            {
                return false;
            }

            // For it to be a valid run, all suites, except wilds
            // must match.  A basic check that this is true to skip
            // further processing
            var distinctCount = grp
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

            var wildCards = grp
                .Where(x => x.Value == (int)round || x.Value == Constants.JOKER)
                .Select(x => x)
                .ToList();

            var orderedList = grp
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
            for (int i = 0; i < grp.Count; i++)
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