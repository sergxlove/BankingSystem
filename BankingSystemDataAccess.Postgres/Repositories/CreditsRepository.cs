using BankingSystemCore.Models;
using BankingSystemDataAccess.Postgres.Abstractions;
using BankingSystemDataAccess.Postgres.Models;
using Microsoft.EntityFrameworkCore;

namespace BankingSystemDataAccess.Postgres.Repositories
{
    public class CreditsRepository : ICreditsRepository
    {
        private readonly BankingSystemDbContext _context;

        public CreditsRepository(BankingSystemDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> CreateAsync(Credits credit, CancellationToken token)
        {
            CreditsEntity creditsEntity = new CreditsEntity()
            {
                Id = credit.Id,
                ClientId = credit.ClientId,
                AccountId = credit.AccountId,
                SumCredit = credit.SumCredit,
                TermMonth = credit.TermMonth,
                StartDate = credit.StartDate,
                EndDate = credit.EndDate,
                PaymentMonth = credit.PaymentMonth,
                LeftCreadit = credit.LeftCredit,
                IsActive = credit.IsActive,
            };
            await _context.Credits.AddAsync(creditsEntity, token);
            await _context.SaveChangesAsync(token);
            return creditsEntity.Id;
        }

        public async Task<int> UpdateLeftCreditAsync(Guid id, decimal newLeftCredit,
            CancellationToken token)
        {
            return await _context.Credits
                .AsNoTracking()
                .Where(a => a.Id == id)
                .ExecuteUpdateAsync(s => s.SetProperty(s => s.LeftCreadit, newLeftCredit), token);
        }

        public async Task<Credits?> GetAsync(Guid id, CancellationToken token)
        {
            var credit = await _context.Credits
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id, token);
            if (credit is null) return null;
            return Credits.Create(credit.Id, credit.ClientId, credit.AccountId, credit.SumCredit,
                credit.TermMonth, credit.StartDate, credit.EndDate, credit.PaymentMonth,
                credit.LeftCreadit, credit.IsActive).Value!;
        }

        public async Task<int> ChangeActiveAsync(Guid id, bool isActive, CancellationToken token)
        {
            return await _context.Credits
                .AsNoTracking()
                .Where(a => a.Id == id)
                .ExecuteUpdateAsync(s => s.SetProperty(s => s.IsActive, isActive), token);
        }

        public async Task<int> DeleteAsync(Guid id, CancellationToken token)
        {
            return await _context.Credits
                .AsNoTracking()
                .Where(a => a.Id == id)
                .ExecuteDeleteAsync(token);
        }
    }
}
