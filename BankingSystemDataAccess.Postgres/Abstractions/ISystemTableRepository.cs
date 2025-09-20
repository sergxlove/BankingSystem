namespace BankingSystemDataAccess.Postgres.Abstractions
{
    public interface ISystemTableRepository
    {
        Task<string> GetAndIncrementAsync();
    }
}