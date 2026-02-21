using BankingSystemCore.Models;
using BankingSystemDataAccess.Postgres.Abstractions;
using BankingSystemDataAccess.Postgres.Models;
using Microsoft.EntityFrameworkCore;

namespace BankingSystemDataAccess.Postgres.Repositories
{
    public class ManagersRepository : IManagersRepository
    {
        private readonly BankingSystemDbContext _context;

        public ManagersRepository(BankingSystemDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> AddAsync(Managers manager, CancellationToken token)
        {
            ManagersEntity managerEntity = new()
            {
                Id = manager.Id,
                ClientSeries = manager.ClientSeries,
                ClientNumbers = manager.ClientNumbers,
                LoginManager = manager.LoginManager,
            };
            await _context.AddAsync(managerEntity, token);
            await _context.SaveChangesAsync(token);
            return managerEntity.Id;
        }

        public async Task<bool> ChechAsync(Managers manager, CancellationToken token)
        {
            ManagersEntity? result = await _context.Managers
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.ClientSeries == manager.ClientSeries
                && a.ClientNumbers == manager.ClientNumbers
                && a.LoginManager == manager.LoginManager, token);
            if (result is null) return false;
            return true;
        }

        public async Task<int> DeleteAsync(Managers manager, CancellationToken token)
        {
            return await _context.Managers
                .AsNoTracking()
                .Where(a => a.ClientSeries == manager.ClientSeries
                && a.ClientNumbers == manager.ClientNumbers
                && a.LoginManager == manager.LoginManager)
                .ExecuteDeleteAsync(token);
        }

        public async Task<string> GetLoginAsync(string passportSeries, string passportNumber,
            CancellationToken token)
        {
            ManagersEntity? result = await _context.Managers
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.ClientSeries == passportSeries
                && a.ClientNumbers == passportNumber, token);
            if (result is null) return string.Empty;
            return result.LoginManager;
        }
    }
}
