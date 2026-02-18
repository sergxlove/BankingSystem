using BankingSystemCore.Models;

namespace BankingSystemDataAccess.Postgres.Abstractions
{
    public interface IUsersRepository
    {
        Task<Guid> CreateAsync(Users user, CancellationToken token);
        Task<bool> VerifyAsync(string username, string password);
        Task<bool> CheckAsync(string username, CancellationToken token);
        Task<string> GetRoleAsync(string username, CancellationToken token);
    }
}