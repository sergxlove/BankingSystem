using BankingSystemApplication.Abstractions;
using BankingSystemDataAccess.Postgres.Abstractions;
using BankingSystemDataAccess.Postgres.Repositories;

namespace BankingSystemApplication.Services
{
    public class SystemTableService : ISystemTableService
    {
        private readonly ISystemTableRepository _repository;
        public SystemTableService(ISystemTableRepository repository)
        {
            _repository = repository;
        }

        public async Task<string> GetAndIncrementAsync()
        {
            return await _repository.GetAndIncrementAsync();
        }
    }
}
