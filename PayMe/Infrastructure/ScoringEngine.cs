using PayMe.Enums;
using PayMe.Models;

namespace PayMe.Infrastructure
{
    public static class ScoringEngine
    {
        public static int ScoreHand(ValidityResult validity, GameRound round)
        {
            var score = 0;
            foreach(var result in validity.Results)
            {
                // This is a valid set, so the score is determined to be 0
                if(result.IsValid)
                {
                    continue;
                }

                foreach(var c in result.Set)
                {
                    // Wilds, Aces and Jokers are 15
                    if(c.Value == (int)round || c.Value == Constants.ACE || c.Value == Constants.JOKER)
                    {
                        score += 15;
                    }
                    // 10s and Faces are 10
                    else if(c.Value >= 10 && c.Value < Constants.ACE)
                    {
                        score += 10;
                    }
                    // Everything else is 5
                    else
                    {
                        score += 5;
                    }
                }
            }

            return score;
        }
    }
}