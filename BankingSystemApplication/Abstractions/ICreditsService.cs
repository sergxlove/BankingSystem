using BankingSystemCore.Models;

namespace BankingSystemApplication.Abstractions
{
    public interface ICreditsService
    {
        Task<int> ChangeActiveAsync(Guid id, bool isActive, CancellationToken token);
        Task<Guid> CreateAsync(Credits credit, CancellationToken token);
        Task<int> DeleteAsync(Guid id, CancellationToken token);
        Task<Credits?> GetAsync(Guid id, CancellationToken token);
        Task<int> UpdateLeftCreditAsync(Guid id, decimal newLeftCredit, CancellationToken token);
    }
}