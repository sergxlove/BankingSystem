namespace BankingSystemDataAccess.Postgres.Models
{
    public class DepositsEntity
    {
        public Guid Id { get; set; }
        public Guid ClientId { get; set; }
        public Guid AccountId { get; set; }
        public decimal SumDeposit { get; set; }
        public int TermMonth { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public int PercentYear { get; set; }
        public bool IsActive { get; set; }

        public virtual ClientsEntity? Clients { get; set; }

        public virtual AccountsEntity? Accounts { get; set; }
    }
}
