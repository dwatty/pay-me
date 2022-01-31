import { GameRound } from "./models/enums";

export class Utils{

    public static GetRoundStrings(round : GameRound) {
        switch(round) {
            case GameRound.Threes:
                return {
                    wildStr: "Threes Wild",
                    roundStr: "3's"
                };
            case GameRound.Fours:
                return {
                    wildStr: "Fours Wild",
                    roundStr: "$'s"
                };
            case GameRound.Fives:
                return {
                    wildStr: "Fives Wild",
                    roundStr: "5's"
                };
            case GameRound.Sixes:
                return {
                    wildStr: "Sixes Wild",
                    roundStr: "6's"
                };
            case GameRound.Sevens:
                return {
                    wildStr: "Sevens Wild",
                    roundStr: "7's"
                };
            case GameRound.Eights:
                return {
                    wildStr: "Eights Wild",
                    roundStr: "8's"
                };
            case GameRound.Nines:
                return {
                    wildStr: "Nines Wild",
                    roundStr: "9's"
                };
            case GameRound.Tens:
                return {
                    wildStr: "Tens Wild",
                    roundStr: "10's"
                };
            case GameRound.Jacks:
                return {
                    wildStr: "Jacks Wild",
                    roundStr: "Jacks"
                };
            case GameRound.Queens:
                return {
                    wildStr: "Queens Wild",
                    roundStr: "Queens"
                };
            case GameRound.Kings:
                return {
                    wildStr: "Kings Wild",
                    roundStr: "Kings"
                };
            case GameRound.Aces:
                return {
                    wildStr: "Aces Wild",
                    roundStr: "Aces"
                };
        }
    }
}