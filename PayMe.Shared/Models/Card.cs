using PayMe.Shared.Enums;

namespace PayMe.Shared.Models;

public class Card
{
    public Guid Id { get; set; }
    public int Value { get; set; }
    public Suites Suite { get; set; }

    public Card(int Value, Suites Suite)
    {
        this.Id = Guid.NewGuid();
        this.Value = Value;
        this.Suite = Suite;
    }

    public override string ToString()
    {
        return $"{this.Value} of {this.Suite.ToString()}";
    }
}

public class WildCard : Card
{
    public int OriginalValue { get; set; }
    public Suites OriginalSuite { get; set; }

    public WildCard(int Value, Suites Suite, int OriginalValue, Suites OriginalSuite) 
        : base(Value, Suite)
    {
        this.OriginalValue = OriginalValue;
        this.OriginalSuite = OriginalSuite;
    }
}