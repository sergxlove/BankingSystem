namespace BankingSystem.Response
{
    public class ShortAccountsResponse
    {
        public string Number { get; set; } = string.Empty;
        public string TypeAccount { get; set; } = string.Empty;
        public string Currency {  get; set; } = string.Empty;
        public decimal Balance {  get; set; } = decimal.Zero;
    }
}
