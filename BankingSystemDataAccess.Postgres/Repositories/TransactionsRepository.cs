using BankingSystemCore.Models;
using BankingSystemDataAccess.Postgres.Abstractions;
using BankingSystemDataAccess.Postgres.Models;
using Microsoft.EntityFrameworkCore;

namespace BankingSystemDataAccess.Postgres.Repositories
{
    public class TransactionsRepository : ITransactionsRepository
    {
        private readonly BankingSystemDbContext _context;

        public TransactionsRepository(BankingSystemDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> CreateAsync(Transactions transaction, CancellationToken token)
        {
            TransactionsEntity transactionsEntity = new TransactionsEntity()
            {
                Id = transaction.Id,
                ProducerAccount = transaction.ProducerAccount,
                ConsumerAccount = transaction.ConsumerAccount,
                TypeOperation = transaction.TypeOperation,
                Amount = transaction.Amount,
                Description = transaction.Description,
                DateCreated = transaction.DateCreated,
            };
            await _context.Transactions.AddAsync(transactionsEntity, token);
            await _context.SaveChangesAsync();
            return transactionsEntity.Id;
        }

        public async Task<Transactions?> GetByIdAsync(Guid id, CancellationToken token)
        {
            var transaction = await _context.Transactions
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id, token);
            if (transaction is null) return null;
            return Transactions.Create(transaction.Id, transaction.ProducerAccount,
                transaction.ConsumerAccount, transaction.TypeOperation, transaction.Amount,
                transaction.Description, transaction.DateCreated).Value!;
        }

        public async Task<List<Transactions>> GetByProducerIdAsync(Guid producerId,
            CancellationToken token)
        {
            var transactions = await _context.Transactions
                .AsNoTracking()
                .Where(a => a.ProducerAccount == producerId)
                .ToListAsync(token);
            List<Transactions> result = new List<Transactions>();
            foreach (var tr in transactions)
            {
                result.Add(Transactions.Create(tr.Id, tr.ProducerAccount, tr.ConsumerAccount,
                    tr.TypeOperation, tr.Amount, tr.Description, tr.DateCreated).Value);
            }
            return result;
        }

        public async Task<List<Transactions>> GetByConsumerAsync(Guid consumerId,
            CancellationToken token)
        {
            var transactions = await _context.Transactions
                .AsNoTracking()
                .Where(a => a.ProducerAccount == consumerId)
                .ToListAsync(token);
            List<Transactions> result = new List<Transactions>();
            foreach (var tr in transactions)
            {
                result.Add(Transactions.Create(tr.Id, tr.ProducerAccount, tr.ConsumerAccount,
                    tr.TypeOperation, tr.Amount, tr.Description, tr.DateCreated).Value);
            }
            return result;
        }
    }
}
