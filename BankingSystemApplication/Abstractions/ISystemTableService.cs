namespace BankingSystemApplication.Abstractions
{
    public interface ISystemTableService
    {
        Task<string> GetAndIncrementAsync();
    }
}