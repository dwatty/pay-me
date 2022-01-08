export enum GameState
{
    AwaitingPlayers,
    InPlay,
    Finished
}

export enum Suites {
    Unknown = -1,
    Hearts = 0,
    Diamonds,
    Clubs,
    Spades,
    Jokers
}

export enum GameRound {
    Threes = 0,
    Fours,
    Fives,
    Sixes,
    Sevens,
    Eights,
    Nines,
    Tens,
    Jacks,
    Queens,
    Kings,
    Aces
}

export enum TurnState {
    NotStarted = 0,
    TurnStarted,
    DrewCard,
    Discarded
}

export enum RoundState {
    Waiting = 0,
    InPlay,
    Finished
}

export enum ClaimResult {
    Valid = 0,
    Invalid
}