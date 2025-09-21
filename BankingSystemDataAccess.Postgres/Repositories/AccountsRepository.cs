using BankingSystemCore.Models;
using BankingSystemDataAccess.Postgres.Abstractions;
using BankingSystemDataAccess.Postgres.Models;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace BankingSystemDataAccess.Postgres.Repositories
{
    public class AccountsRepository : IAccountsRepository
    {
        private readonly BankingSystemDbContext _context;

        public AccountsRepository(BankingSystemDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> CreateAsync(Accounts account, CancellationToken token)
        {
            try
            {
                AccountsEntity accountsEntity = new AccountsEntity()
                {
                    Id = account.Id,
                    ClientsId = account.ClientsId,
                    AccountType = account.AccountType,
                    AccountNumber = account.AccountNumber,
                    Balance = account.Balance,
                    CurrencyCode = account.CurrencyCode,
                    OpenDate = account.OpenDate,
                    CloseDate = account.CloseDate,
                    IsActive = account.IsActive,
                };
                await _context.AddAsync(accountsEntity, token);
                await _context.SaveChangesAsync(token);
                return accountsEntity.Id;
            }
            catch
            {
                return Guid.Empty;
            }
        }

        public async Task<int> UpdateBalanceAsync(Guid id, decimal newBalance, CancellationToken token)
        {
            return await _context.Accounts
                .AsNoTracking()
                .Where(a => a.Id == id)
                .ExecuteUpdateAsync(s => s.SetProperty(s => s.Balance, newBalance), token);
        }

        public async Task<Accounts?> GetAsync(Guid id, CancellationToken token)
        {
            var account = await _context.Accounts
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id, token);
            if (account is null) return null;
            return Accounts.Create(account.Id, account.ClientsId, account.AccountType,
                account.AccountNumber, account.Balance, account.CurrencyCode, account.OpenDate,
                account.CloseDate, account.IsActive).Value!;
        }

        public async Task<List<Accounts>> GetListAsync(Guid idClient,  CancellationToken token)
        {
            var accounts = await _context.Accounts
                .AsNoTracking()
                .Where (a => a.ClientsId == idClient)
                .ToListAsync(token);
            List<Accounts> result = new List<Accounts>();
            foreach(var ak in accounts)
            {
                result.Add(Accounts.Create(ak.Id, ak.ClientsId, ak.AccountType, ak.AccountNumber, 
                    ak.Balance, ak.CurrencyCode, ak.OpenDate, ak.CloseDate, ak.IsActive).Value);
            }
            return result;
        }

        public async Task<int> ChangeActiveAsync(Guid id, bool isActive, CancellationToken token)
        {
            return await _context.Accounts
                .AsNoTracking()
                .Where(a => a.Id == id)
                .ExecuteUpdateAsync(s => s.SetProperty(s => s.IsActive, isActive), token);
        }

        public async Task<int> DeleteAsync(Guid id, CancellationToken token)
        {
            return await _context.Accounts
                .AsNoTracking()
                .Where(a => a.Id == id)
                .ExecuteDeleteAsync(token);
        }
    }
}
