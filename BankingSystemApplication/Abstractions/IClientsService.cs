using BankingSystemCore.Models;
using BankingSystemDataAccess.Postgres.Dto;

namespace BankingSystemApplication.Abstractions
{
    public interface IClientsService
    {
        Task<Guid> CreateAsync(Clients client, CancellationToken token);
        Task<int> DeleteAsync(Guid id, CancellationToken token);
        Task<Clients?> GetAsync(string passportSeries, string passportNumber, CancellationToken token);
        Task<Guid> GetIdAsync(string passportSeries, string passportNumber, CancellationToken token);
        Task<int> UpdateAsync(Clients clients, CancellationToken token);
        Task<bool> CheckAsync(string passportSeries, string passportNumber, CancellationToken token);
        Task<List<ClientBalanceDto>> GetClientsByBalanceRangeAsync(decimal minBalance, decimal maxBalance, CancellationToken token);
        Task<List<ClientCreditDto>> GetBorrowersByMonthsLeftAsync(int maxMonthsLeft, CancellationToken token);
    }
}