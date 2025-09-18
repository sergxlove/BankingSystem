using BankingSystemCore.Models;
using BankingSystemDataAccess.Postgres.Abstractions;
using BankingSystemDataAccess.Postgres.Models;
using Microsoft.EntityFrameworkCore;

namespace BankingSystemDataAccess.Postgres.Repositories
{
    public class DepositsRepository : IDepositsRepository
    {
        private readonly BankingSystemDbContext _context;

        public DepositsRepository(BankingSystemDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> CreateAsync(Deposits deposit, CancellationToken token)
        {
            DepositsEntity depositEntity = new DepositsEntity()
            {
                Id = deposit.Id,
                ClientId = deposit.ClientId,
                AccountId = deposit.AccountId,
                SumDeposit = deposit.SumDeposit,
                TermMonth = deposit.TermMonth,
                StartDate = deposit.StartDate,
                EndDate = deposit.EndDate,
                PercentYear = deposit.PercentYear,
                IsActive = deposit.IsActive,
            };
            await _context.Deposits.AddAsync(depositEntity, token);
            await _context.SaveChangesAsync(token);
            return depositEntity.Id;
        }

        public async Task<Deposits?> GetAsync(Guid id, CancellationToken token)
        {
            var deposit = await _context.Deposits
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id, token);
            if (deposit is null) return null;
            return Deposits.Create(deposit.Id, deposit.ClientId, deposit.AccountId,
                deposit.SumDeposit, deposit.TermMonth, deposit.StartDate, deposit.EndDate,
                deposit.PercentYear, deposit.IsActive).Value!;
        }

        public async Task<int> ChangeActiveAsync(Guid id, bool isActive, CancellationToken token)
        {
            return await _context.Deposits
                .AsNoTracking()
                .Where(a => a.Id == id)
                .ExecuteUpdateAsync(s => s.SetProperty(s => s.IsActive, isActive), token);
        }

        public async Task<int> DeleteAsync(Guid id, CancellationToken token)
        {
            return await _context.Deposits
                .AsNoTracking()
                .Where(a => a.Id == id)
                .ExecuteDeleteAsync(token);
        }
    }
}
