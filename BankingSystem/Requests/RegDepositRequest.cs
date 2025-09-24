namespace BankingSystem.Requests
{
    public class RegDepositRequest
    {
        public Guid ClientId { get; set; }
        public Guid AccountId { get; set; }
        public decimal SumDeposit { get; set; }
        public int TermMonth { get; set; }
    }
}
