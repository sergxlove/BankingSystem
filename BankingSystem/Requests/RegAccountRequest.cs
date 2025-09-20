namespace BankingSystem.Requests
{
    public class RegAccountRequest
    {
        public Guid ClientsId { get; set; }
        public string AccountType { get; set; } = string.Empty;
        public decimal Balance { get; set; }
        public string CurrencyCode { get; set; } = string.Empty;
    }
}
