namespace BankingSystem.Requests
{
    public class TransactRequest
    {
        public Guid ProducerAccount {  get; set; }
        public Guid ConsumerAccount { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
