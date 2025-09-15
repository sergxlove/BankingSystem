namespace BankingSystemDataAccess.Postgres.Models
{
    public class TransactionsEntity
    {
        public Guid Id { get; set; }
        public Guid ProducerAccount {  get; set; }
        public Guid ConsumerAccount { get; set; }
        public Guid TypeOperation { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; } = string.Empty; 
        public DateOnly DateCreated { get; set; }

        public virtual OperationsTransactionsEntity? OperationsTransactions { get; set; }
    }
}
