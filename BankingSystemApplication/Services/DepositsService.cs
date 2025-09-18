using BankingSystemApplication.Abstractions;
using BankingSystemCore.Models;
using BankingSystemDataAccess.Postgres.Abstractions;

namespace BankingSystemApplication.Services
{
    public class DepositsService : IDepositsService
    {
        private readonly IDepositsRepository _repository;

        public DepositsService(IDepositsRepository repository)
        {
            _repository = repository;
        }

        public async Task<int> ChangeActiveAsync(Guid id, bool isActive, CancellationToken token)
        {
            return await _repository.ChangeActiveAsync(id, isActive, token);
        }
        public async Task<Guid> CreateAsync(Deposits deposit, CancellationToken token)
        {
            return await _repository.CreateAsync(deposit, token);
        }
        public async Task<int> DeleteAsync(Guid id, CancellationToken token)
        {
            return await _repository.DeleteAsync(id, token);
        }
        public async Task<Deposits?> GetAsync(Guid id, CancellationToken token)
        {
            return await _repository.GetAsync(id, token);
        }
    }
}
