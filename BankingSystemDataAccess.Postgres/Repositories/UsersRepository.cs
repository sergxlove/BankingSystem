using BankingSystemCore.Models;
using BankingSystemDataAccess.Postgres.Abstractions;
using BankingSystemDataAccess.Postgres.Models;
using Microsoft.EntityFrameworkCore;

namespace BankingSystemDataAccess.Postgres.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly BankingSystemDbContext _context;

        public UsersRepository(BankingSystemDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> CreateAsync(Users user, CancellationToken token)
        {
            try
            {
                UsersEntity usersEntity = new UsersEntity()
                {
                    Id = user.Id,
                    Username = user.Username,
                    HashPassword = user.HashPassword,
                    Role = user.Role
                };
                await _context.Users.AddAsync(usersEntity, token);
                await _context.SaveChangesAsync(token);
                return usersEntity.Id;
            }
            catch
            {
                return Guid.Empty;
            }
        }

        public async Task<bool> VerifyAsync(string username, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(a => a.Username == username);
            if (user == null) return false;
            return Users.VerifyPassword(password, user.HashPassword);
        }

        public async Task<bool> CheckAsync(string username, CancellationToken token)
        {
            var user = await _context.Users.FirstOrDefaultAsync(a => a.Username == username, token);
            if(user is null) return false;
            return true;
        }
    }
}
