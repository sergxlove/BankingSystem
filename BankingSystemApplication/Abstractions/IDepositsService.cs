using BankingSystemCore.Models;

namespace BankingSystemApplication.Abstractions
{
    public interface IDepositsService
    {
        Task<int> ChangeActiveAsync(Guid id, bool isActive, CancellationToken token);
        Task<Guid> CreateAsync(Deposits deposit, CancellationToken token);
        Task<int> DeleteAsync(Guid id, CancellationToken token);
        Task<Deposits?> GetAsync(Guid id, CancellationToken token);
    }
}