namespace BankingSystemDataAccess.Postgres.Models
{
    public class AccountsEntity
    {
        public Guid Id { get; set; }
        public Guid ClientsId { get; set; }
        public string AccountType { get; set; } = string.Empty;
        public string AccountNumber { get; set; } = string.Empty;
        public decimal Balance { get; set; }
        public string CurrencyCode { get; set; } = string.Empty;
        public DateOnly OpenDate { get; set; }
        public DateOnly CloseDate { get; set; }
        public bool IsActive {  get; set; }

        public virtual ClientsEntity? Clients { get; set; }

        public virtual List<CreditsEntity> Credits { get; set; } = new();

        public virtual List<DepositsEntity> Deposits { get; set; } = new();
    }
}
