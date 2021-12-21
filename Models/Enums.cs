namespace PayMe.Enums
{
    public enum Suites
    {
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
    public enum GameAction
    {
        PickDiscard = 0,
        DrawCard,
        Discard,
        GoOut
    }

    [Serializable]
    public enum GameRound
    {
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

    public enum TurnState
    {
        NotStarted = 0,
        TurnStarted,
        DrewCard,
        Discarded
    }
}