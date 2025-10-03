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
        Task<List<Accounts>> GetListAsync(Guid idClient, CancellationToken token);
        Task<decimal> GetCurrentBalanceAsync(Guid id, CancellationToken token);
        Task<bool> CheckAsync(Guid id, CancellationToken token);
    }
}