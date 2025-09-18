using BankingSystemApplication.Abstractions;
using BankingSystemCore.Models;
using BankingSystemDataAccess.Postgres.Abstractions;

namespace BankingSystemApplication.Services
{
    public class UsersService : IUsersService
    {
        private readonly IUsersRepository _repository;
        public UsersService(IUsersRepository repository)
        {
            _repository = repository;
        }

        public async Task<Guid> CreateAsync(Users user, CancellationToken token)
        {
            return await _repository.CreateAsync(user, token);
        }
        public async Task<bool> VerifyAsync(string username, string password)
        {
            return await _repository.VerifyAsync(username, password);
        }
    }
}
