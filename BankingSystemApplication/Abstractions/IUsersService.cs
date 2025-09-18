using BankingSystemCore.Models;

namespace BankingSystemApplication.Abstractions
{
    public interface IUsersService
    {
        Task<Guid> CreateAsync(Users user, CancellationToken token);
        Task<bool> VerifyAsync(string username, string password);
    }
}