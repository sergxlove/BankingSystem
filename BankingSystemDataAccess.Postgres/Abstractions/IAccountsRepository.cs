using BankingSystemCore.Models;

namespace BankingSystemDataAccess.Postgres.Abstractions
{
    public interface IAccountsRepository
    {
        Task<int> ChangeActiveAsync(Guid id, bool isActive, CancellationToken token);
        Task<Guid> CreateAsync(Accounts account, CancellationToken token);
        Task<int> DeleteAsync(Guid id, CancellationToken token);
        Task<Accounts?> GetAsync(Guid id, CancellationToken token);
        Task<int> UpdateBalanceAsync(Guid id, decimal newBalance, CancellationToken token);
    }
}