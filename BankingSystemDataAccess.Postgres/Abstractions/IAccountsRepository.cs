using BankingSystemCore.Models;

namespace BankingSystemDataAccess.Postgres.Abstractions
{
    public interface IAccountsRepository
    {
        Task<int> ChangeActiveAsync(Guid id, bool isActive);
        Task<Guid> CreateAsync(Accounts account, CancellationToken token);
        Task<int> DeleteAsync(Guid id);
        Task<Accounts?> GetAsync(Guid id);
        Task<int> UpdateBalanceAsync(Guid id, decimal newBalance);
    }
}