using BankingSystemCore.Models;

namespace BankingSystemDataAccess.Postgres.Abstractions
{
    public interface IDepositsRepository
    {
        Task<int> ChangeActiveAsync(Guid id, bool isActive, CancellationToken token);
        Task<Guid> CreateAsync(Deposits deposit, CancellationToken token);
        Task<int> DeleteAsync(Guid id, CancellationToken token);
        Task<Deposits?> GetAsync(Guid id, CancellationToken token);
    }
}