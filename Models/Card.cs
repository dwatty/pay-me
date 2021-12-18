using PayMe.Enums;

namespace PayMe.Models
{
    public class Card
    {
        public int Value { get; set; }
        public Suites Suite { get; set; }

        public Card(int Value, Suites Suite)
        {
            this.Value = Value;
            this.Suite = Suite;
        }
    }
}