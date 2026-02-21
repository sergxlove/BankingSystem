using BankingSystemCore.Models;

namespace BankingSystemApplication.Abstractions
{
    public interface IManagersService
    {
        Task<Guid> AddAsync(Managers manager, CancellationToken token);
        Task<bool> ChechAsync(Managers manager, CancellationToken token);
        Task<int> DeleteAsync(Managers manager, CancellationToken token);
        Task<string> GetLoginAsync(string passportSeries, string passportNumber, CancellationToken token);
    }
}