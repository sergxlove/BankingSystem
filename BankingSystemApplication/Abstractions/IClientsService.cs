using BankingSystemCore.Models;

namespace BankingSystemApplication.Abstractions
{
    public interface IClientsService
    {
        Task<Guid> CreateAsync(Clients client, CancellationToken token);
        Task<int> DeleteAsync(Guid id, CancellationToken token);
        Task<Clients?> GetAsync(string passportSeries, string passportNumber, CancellationToken token);
        Task<Guid> GetIdAsync(string passportSeries, string passportNumber, CancellationToken token);
        Task<int> UpdateAsync(Clients clients, CancellationToken token);
    }
}