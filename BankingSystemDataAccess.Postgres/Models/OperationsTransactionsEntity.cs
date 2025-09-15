namespace BankingSystemDataAccess.Postgres.Models
{
    public class OperationsTransactionsEntity
    {
        public Guid Id { get; set; }
        public string TypeOperation {  get; set; } = string.Empty;
        public string Description {  get; set; } = string.Empty;

        public virtual List<TransactionsEntity> Transactions { get; set; } = new();
    }
}
