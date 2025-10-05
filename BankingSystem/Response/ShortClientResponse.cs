namespace BankingSystem.Response
{
    public class ShortClientResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateOnly DateBirth { get; set; }
    }
}
