using BankingSystemApplication.Abstractions;
using BankingSystemCore.Models;
using BankingSystemDataAccess.Postgres.Abstractions;

namespace BankingSystemApplication.Services
{
    public class CreditsService : ICreditsService
    {
        private readonly ICreditsRepository _repository;
        public CreditsService(ICreditsRepository repository)
        {
            _repository = repository;
        }

        public async Task<int> ChangeActiveAsync(Guid id, bool isActive, CancellationToken token)
        {
            return await _repository.ChangeActiveAsync(id, isActive, token);
        }
        public async Task<Guid> CreateAsync(Credits credit, CancellationToken token)
        {
            return await _repository.CreateAsync(credit, token);
        }
        public async Task<int> DeleteAsync(Guid id, CancellationToken token)
        {
            return await _repository.DeleteAsync(id, token);
        }
        public async Task<Credits?> GetAsync(Guid id, CancellationToken token)
        {
            return await _repository.GetAsync(id, token);
        }

        public async Task<List<Credits>> GetListAsync(Guid clientId, CancellationToken token)
        {
            return await _repository.GetListAsync(clientId, token);
        }
        public async Task<int> UpdateLeftCreditAsync(Guid id, decimal newLeftCredit,
            CancellationToken token)
        {
            return await _repository.UpdateLeftCreditAsync(id, newLeftCredit, token);
        }
    }
}
