using BankingSystemApplication.Requests;

namespace BankingSystemApplication.Abstractions
{
    public interface IJwtProviderService
    {
        string? GenerateToken(JwtRequest request);
    }
}