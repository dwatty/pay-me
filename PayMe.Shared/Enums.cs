namespace PayMe.Shared.Enums;

public enum Suites
{
    Unknown = -1,
    Hearts = 0,
    Diamonds,
    Clubs,
    Spades,
    Jokers
}


[Serializable]
public enum GameState
{
    AwaitingPlayers,
    InPlay,
    Finished
}

[Serializable]
public enum GameOutcome
{
    Win,
    Lose,
    Draw
}

[Serializable]
public enum GameEvents
{
    Unknown = 0,
    GameStarted,
    PlayerAdded,
    RoundOver,
    ScoreResult,
    DrawCard,
    DrawDiscard,
    Discard,
    ClaimWin,
    ClaimFail,
    GetSumamry,
    SetName,
    HandDealt
}

[Serializable]
public enum GameRound
{
    Threes = 3,
    Fours = 4,
    Fives = 5,
    Sixes = 6,
    Sevens = 7,
    Eights = 8,
    Nines = 9,
    Tens = 10,
    Jacks = 11,
    Queens = 12,
    Kings = 13,
    Aces = 14
}

public enum TurnState
{
    NotStarted = 0,
    TurnStarted,
    DrewCard,
    Discarded
}

public enum RoundState
{
    Waiting = 0,
    InPlay,
    Finished

}


public enum ClaimResult
{
    Valid = 0,
    Invalid
}