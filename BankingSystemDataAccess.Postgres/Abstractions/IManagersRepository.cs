using BankingSystemCore.Models;

namespace BankingSystemDataAccess.Postgres.Abstractions
{
    public interface IManagersRepository
    {
        Task<Guid> AddAsync(Managers manager, CancellationToken token);
        Task<bool> ChechAsync(Managers manager, CancellationToken token);
        Task<int> DeleteAsync(Managers manager, CancellationToken token);
        Task<string> GetLoginAsync(string passportSeries, string passportNumber, CancellationToken token);
    }
}