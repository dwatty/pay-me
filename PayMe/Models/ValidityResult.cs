namespace PayMe.Models;

public class ValidityResult
{
    public List<SetValidityResult> Results { get; set; } = new List<SetValidityResult>();
    public bool IsInvalidCollection { get; set; }

    public bool AllSetsValid() => this.Results.All(x => x.IsValid);
}

public class SetValidityResult
{
    public List<Card> Set { get; set; } = new List<Card>();
    public bool IsValid { get; set; }

    public SetValidityResult(List<Card> set) 
    {
        this.Set = set;
    }

    public SetValidityResult(List<Card> Set, bool IsValid)
    {
        this.Set = Set;
        this.IsValid = IsValid;
    }
}