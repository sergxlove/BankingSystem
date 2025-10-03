using BankingSystemCore.Models;

namespace BankingSystemApplication.Abstractions
{
    public interface ITransactMoneyService
    {
        Task<string> ExecuteTransactAsync(Transactions transaction, CancellationToken token);
    }
}