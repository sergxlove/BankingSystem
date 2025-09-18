using BankingSystemApplication.Abstractions;
using BankingSystemCore.Models;
using BankingSystemDataAccess.Postgres.Abstractions;

namespace BankingSystemApplication.Services
{
    public class ClientsService : IClientsService
    {
        private readonly IClientsRepository _repository;

        public ClientsService(IClientsRepository repository)
        {
            _repository = repository;
        }

        public async Task<Guid> CreateAsync(Clients client, CancellationToken token)
        {
            return await _repository.CreateAsync(client, token);
        }
        public async Task<int> DeleteAsync(Guid id, CancellationToken token)
        {
            return await _repository.DeleteAsync(id, token);
        }
        public async Task<Clients?> GetAsync(string passportSeries, string passportNumber,
            CancellationToken token)
        {
            return await _repository.GetAsync(passportSeries, passportNumber, token);
        }
        public async Task<Guid> GetIdAsync(string passportSeries, string passportNumber,
            CancellationToken token)
        {
            return await _repository.GetIdAsync(passportSeries, passportNumber, token);
        }
        public async Task<int> UpdateAsync(Clients clients, CancellationToken token)
        {
            return await _repository.UpdateAsync(clients, token);
        }
    }
}
