using BankingSystem.Endpoints;

namespace BankingSystem.Extensions
{
    public static class RegistrEndpoints
    {
        public static IEndpointRouteBuilder MapAllEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapBankingOperationsEndpoints();
            app.MapGetPagesEndpoints();
            return app;
        }
    }
}
