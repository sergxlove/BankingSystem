namespace BankingSystem.Requests
{
    public class GetClientRequest
    {
        public string PassportSeries { get; set; } = string.Empty;
        public string PassportNumber { get; set; } = string.Empty;
    }
}
