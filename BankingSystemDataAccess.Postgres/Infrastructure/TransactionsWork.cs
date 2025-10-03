using BankingSystemDataAccess.Postgres.Abstractions;
using Microsoft.EntityFrameworkCore.Storage;

namespace BankingSystemDataAccess.Postgres.Infrastructure
{
    public class TransactionsWork : ITransactionsWork
    {
        private readonly BankingSystemDbContext _context;
        private IDbContextTransaction? _transaction;

        public TransactionsWork(BankingSystemDbContext context)
        {
            _context = context;
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            await _transaction!.CommitAsync();
        }

        public async Task RollbackAsync()
        {
            await _transaction!.RollbackAsync();
        }
    }
}
