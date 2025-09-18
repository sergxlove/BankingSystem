using BankingSystemCore.Models;

namespace BankingSystemDataAccess.Postgres.Abstractions
{
    public interface ITransactionsRepository
    {
        Task<Guid> CreateAsync(Transactions transaction, CancellationToken token);
        Task<List<Transactions>> GetByConsumerAsync(Guid consumerId, CancellationToken token);
        Task<Transactions?> GetByIdAsync(Guid id, CancellationToken token);
        Task<List<Transactions>> GetByProducerIdAsync(Guid producerId, CancellationToken token);
    }
}