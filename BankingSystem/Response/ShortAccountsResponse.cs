namespace BankingSystem.Response
{
    public class ShortAccountsResponse
    {
        public Guid Id { get; set; }
        public string Number { get; set; } = string.Empty;
        public string TypeAccount { get; set; } = string.Empty;
        public string Currency {  get; set; } = string.Empty;
        public decimal Balance {  get; set; } = decimal.Zero;
    }
}
