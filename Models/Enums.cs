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
        Twos = 0,
        Threes,
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
}