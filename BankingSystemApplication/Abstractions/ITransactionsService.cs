using BankingSystemCore.Models;

namespace BankingSystemApplication.Abstractions
{
    public interface ITransactionsService
    {
        Task<Guid> CreateAsync(Transactions transaction, CancellationToken token);
        Task<List<Transactions>> GetByConsumerAsync(Guid consumerId, CancellationToken token);
        Task<Transactions?> GetByIdAsync(Guid id, CancellationToken token);
        Task<List<Transactions>> GetByProducerIdAsync(Guid producerId, CancellationToken token);
    }
}