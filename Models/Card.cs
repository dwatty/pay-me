using PayMe.Enums;

namespace PayMe.Models
{
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
    }
}