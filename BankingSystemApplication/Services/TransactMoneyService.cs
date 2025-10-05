using BankingSystemApplication.Abstractions;
using BankingSystemCore.Models;
using BankingSystemDataAccess.Postgres.Abstractions;

namespace BankingSystemApplication.Services
{
    public class TransactMoneyService : ITransactMoneyService
    {
        private readonly IAccountsRepository _accountRepository;
        private readonly ITransactionsWork _transactionsWork;
        private readonly ITransactionsRepository _transactionsRepository;

        public TransactMoneyService(IAccountsRepository accountsRepository,
            ITransactionsWork transactionsWork, ITransactionsRepository transactionsRepository)
        {
            _accountRepository = accountsRepository;
            _transactionsWork = transactionsWork;
            _transactionsRepository = transactionsRepository;
        }

        public async Task<string> ExecuteTransactAsync(Transactions transaction, CancellationToken token)
        {
            try
            {
                await _transactionsWork.BeginTransactionAsync();
                if (!await _accountRepository.CheckAsync(transaction.ConsumerAccount, token))
                    throw new Exception("consumer account is not found");
                if (!await _accountRepository.CheckAsync(transaction.ProducerAccount, token))
                    throw new Exception("producer account is not fount");
                decimal balanceProducer = await _accountRepository
                    .GetCurrentBalanceAsync(transaction.ProducerAccount, token);
                if (balanceProducer < transaction.Amount)
                    throw new Exception("insufficient funds");
                decimal balanceConsumer = await _accountRepository
                    .GetCurrentBalanceAsync(transaction.ConsumerAccount, token);
                var updateProducer = await _accountRepository.UpdateBalanceAsync(transaction.ProducerAccount,
                    balanceProducer - transaction.Amount, token);
                var updateConsumer = await _accountRepository.UpdateBalanceAsync(transaction.ConsumerAccount,
                    balanceConsumer + transaction.Amount, token);
                if (updateProducer == 0 || updateConsumer == 0) throw new Exception("error transact");
                await _transactionsRepository.CreateAsync(transaction, token);
                await _transactionsWork.CommitAsync();
                return string.Empty;
            }
            catch (Exception ex)
            {
                await _transactionsWork.RollbackAsync();
                return ex.Message;
            }
        }
    }
}
