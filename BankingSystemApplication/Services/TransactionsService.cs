using BankingSystemApplication.Abstractions;
using BankingSystemCore.Models;
using BankingSystemDataAccess.Postgres.Abstractions;

namespace BankingSystemApplication.Services
{
    public class TransactionsService : ITransactionsService
    {
        private readonly ITransactionsRepository _repository;

        public TransactionsService(ITransactionsRepository repository)
        {
            _repository = repository;
        }

        public async Task<Guid> CreateAsync(Transactions transaction, CancellationToken token)
        {
            return await _repository.CreateAsync(transaction, token);
        }
        public async Task<List<Transactions>> GetByConsumerAsync(Guid consumerId,
            CancellationToken token)
        {
            return await _repository.GetByConsumerAsync(consumerId, token);
        }
        public async Task<Transactions?> GetByIdAsync(Guid id, CancellationToken token)
        {
            return await _repository.GetByIdAsync(id, token);
        }
        public async Task<List<Transactions>> GetByProducerIdAsync(Guid producerId,
            CancellationToken token)
        {
            return await _repository.GetByProducerIdAsync(producerId, token);
        }
    }
}
