namespace BankingSystemDataAccess.Postgres.Abstractions
{
    public interface ITransactionsWork
    {
        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();
    }
}