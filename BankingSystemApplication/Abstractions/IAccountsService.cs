using BankingSystemCore.Models;

namespace BankingSystemApplication.Abstractions
{
    public interface IAccountsService
    {
        Task<int> ChangeActiveAsync(Guid id, bool isActive, CancellationToken token);
        Task<Guid> CreateAsync(Accounts account, CancellationToken token);
        Task<int> DeleteAsync(Guid id, CancellationToken token);
        Task<Accounts?> GetAsync(Guid id, CancellationToken token);
        Task<int> UpdateBalanceAsync(Guid id, decimal newBalance, CancellationToken token);
        Task<List<Accounts>> GetListAsync(Guid idClient, CancellationToken token);
    }
}