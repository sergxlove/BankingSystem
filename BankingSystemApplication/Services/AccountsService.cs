using BankingSystemApplication.Abstractions;
using BankingSystemCore.Models;
using BankingSystemDataAccess.Postgres.Abstractions;

namespace BankingSystemApplication.Services
{
    public class AccountsService : IAccountsService
    {
        private readonly IAccountsRepository _repository;
        public AccountsService(IAccountsRepository repository)
        {
            _repository = repository;
        }

        public async Task<int> ChangeActiveAsync(Guid id, bool isActive, CancellationToken token)
        {
            return await _repository.ChangeActiveAsync(id, isActive, token);
        }
        public async Task<Guid> CreateAsync(Accounts account, CancellationToken token)
        {
            return await _repository.CreateAsync(account, token);
        }
        public async Task<int> DeleteAsync(Guid id, CancellationToken token)
        {
            return await _repository.DeleteAsync(id, token);
        }
        public async Task<Accounts?> GetAsync(Guid id, CancellationToken token)
        {
            return await _repository.GetAsync(id, token);
        }
        public async Task<int> UpdateBalanceAsync(Guid id, decimal newBalance, CancellationToken token)
        {
            return await _repository.UpdateBalanceAsync(id, newBalance, token);
        }
        public async Task<List<Accounts>> GetListAsync(Guid idClient, CancellationToken token)
        {
            return await _repository.GetListAsync(idClient, token);
        }
    }
}
