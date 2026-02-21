using BankingSystemApplication.Abstractions;
using BankingSystemCore.Models;
using BankingSystemDataAccess.Postgres.Abstractions;

namespace BankingSystemApplication.Services
{
    public class ManagersService : IManagersService
    {
        private readonly IManagersRepository _repository;
        public ManagersService(IManagersRepository repository)
        {
            _repository = repository;
        }
        public async Task<Guid> AddAsync(Managers manager, CancellationToken token)
        {
            return await _repository.AddAsync(manager, token);
        }
        public async Task<bool> ChechAsync(Managers manager, CancellationToken token)
        {
            return await _repository.ChechAsync(manager, token);
        }
        public async Task<int> DeleteAsync(Managers manager, CancellationToken token)
        {
            return await _repository.DeleteAsync(manager, token);
        }
        public async Task<string> GetLoginAsync(string passportSeries, string passportNumber,
            CancellationToken token)
        {
            return await GetLoginAsync(passportSeries, passportNumber, token);
        }
    }
}
