using BankingSystem.Requests;
using BankingSystem.Responce;
using BankingSystemApplication.Abstractions;
using BankingSystemApplication.Requests;
using BankingSystemCore.Abstractions;
using BankingSystemCore.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BankingSystem.Endpoints
{
    public static class BankingOperationsEndpoints
    {
        public static IEndpointRouteBuilder MapBankingOperationsEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/", () =>
            {
                return Results.Redirect("/login");
            }).RequireRateLimiting("GeneralPolicy");

            app.MapGet("/index", async (HttpContext context) =>
            {
                context.Response.ContentType = "text/html; charset=utf-8";
                await context.Response.SendFileAsync("wwwroot/index.html");
            }).RequireAuthorization("OnlyForAuthUser")
            .RequireRateLimiting("GeneralPolicy");

            app.MapGet("/login", async (HttpContext context) =>
            {
                context.Response.ContentType = "text/html; charset=utf-8";
                await context.Response.SendFileAsync("wwwroot/login.html");
            }).RequireRateLimiting("GeneralPolicy");

            app.MapGet("/reg", async (HttpContext context) =>
            {
                context.Response.ContentType = "text/html; charset=utf-8";
                await context.Response.SendFileAsync("wwwroot/reg.html");
            }).RequireRateLimiting("GeneralPolicy");

            app.MapPost("/login", async (HttpContext context,
                [FromBody] LoginRequest request,
                [FromServices] IUsersService userService,
                [FromServices] IJwtProviderService jwtService,
                CancellationToken token) =>
            {
                try
                {
                    if (request.Username == string.Empty || request.Password == string.Empty)
                        return Results.BadRequest("login or password is empty");
                    if (!await userService.VerifyAsync(request.Username, request.Password))
                        return Results.BadRequest("no auth");
                    var claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.Role, "user"),
                    };
                    var jwttoken = jwtService.GenerateToken(new JwtRequest()
                    {
                        Claims = claims
                    });
                    context.Response.Cookies.Append("jwt", jwttoken!);
                    return Results.Ok();
                }
                catch
                {
                    return Results.InternalServerError();
                }
            }).RequireRateLimiting("LoginPolicy");

            app.MapPost("/reg", async (HttpContext context,
                [FromBody] RegistrRequest request,
                [FromServices] IUsersService userService,
                [FromServices] IJwtProviderService jwtService,
                [FromServices] IPasswordHasherService passwordHasher,
                CancellationToken token) =>
            {
                try
                {
                    if (request.Username == string.Empty || request.Password == string.Empty ||
                        request.AgainPassword == string.Empty)
                        return Results.BadRequest("login or password is empty");
                    if (request.Password != request.AgainPassword)
                        return Results.BadRequest("passwords is not equals");
                    var user = Users.Create(Guid.NewGuid(), request.Username, request.Password,
                        "user", passwordHasher);
                    if (!user.IsSuccess) return Results.BadRequest(user.Error);
                    var result = await userService.CreateAsync(user.Value, token);
                    var claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.Role, "user"),
                    };
                    var jwttoken = jwtService.GenerateToken(new JwtRequest()
                    {
                        Claims = claims
                    });
                    context.Response.Cookies.Append("jwt", jwttoken!);
                    return Results.Ok();
                }
                catch
                {
                    return Results.InternalServerError();
                }
            }).RequireRateLimiting("GeneralPolicy");

            app.MapPost("/regClient", async (HttpContext context, 
                [FromBody] ClientsRequest request,
                [FromServices] IClientsService clientService, 
                CancellationToken token) =>
            {
                try
                {
                    if (request is null) return Results.BadRequest("request is empty");
                    var client = Clients.Create(Guid.NewGuid(), request.FirstName, request.SecondName,
                        request.LastName, request.BirthDate, request.PassportSeries, 
                        request.PassportNumber, request.PhoneNumber, request.EmailAddress, 
                        request.AddressRegistration, DateOnly.FromDateTime(DateTime.UtcNow));
                    if (!client.IsSuccess) return Results.BadRequest(client.Error);
                    var result = await clientService.CreateAsync(client.Value, token);
                    if(result == client.Value.Id) return Results.Ok();
                    return Results.InternalServerError();
                }
                catch
                {
                    return Results.InternalServerError();
                }
            }).RequireAuthorization("OnlyForAuthUser")
            .RequireRateLimiting("GeneralPolicy");

            app.MapPost("/getClient", async (HttpContext context,
                [FromBody] GetClientRequest request,
                [FromServices] IClientsService clientService,
                CancellationToken token) =>
            {
                try
                {
                    if (request.PassportSeries == string.Empty || request.PassportNumber == string.Empty)
                        return Results.BadRequest("passport data is empty");
                    var idClient = await clientService.GetIdAsync(request.PassportSeries, 
                        request.PassportNumber, token);
                    if (idClient == Guid.Empty) return Results.BadRequest("client is not found");
                    return Results.Ok(idClient);
                }
                catch
                {
                    return Results.InternalServerError();
                }
            }).RequireAuthorization("OnlyForAuthUser")
            .RequireRateLimiting("GeneralPolicy");

            app.MapPost("/regAccount", async (HttpContext context, 
                [FromBody] RegAccountRequest request, 
                [FromServices] IAccountsService accountsService,
                [FromServices] ISystemTableService systemTableService,
                CancellationToken token) =>
            {
                try
                {
                    if (request is null) return Results.BadRequest("request is empty");
                    string numberCard = await systemTableService.GetAndIncrementAsync();
                    var account = Accounts.Create(Guid.NewGuid(), request.ClientsId, 
                        request.AccountType, numberCard, request.Balance, request.CurrencyCode,
                        DateOnly.FromDateTime(DateTime.Now), DateOnly.MaxValue, true);
                    if (!account.IsSuccess) return Results.BadRequest(account.Error);
                    var result = await accountsService.CreateAsync(account.Value, token);
                    return Results.Ok();
                }
                catch
                {
                    return Results.InternalServerError();
                }

            }).RequireAuthorization("OnlyForAuthUser")
            .RequireRateLimiting("GeneralPolicy");

            app.MapPost("/regCredit", async (HttpContext context, 
                [FromBody] RegCreditRequest request, 
                [FromServices] ICreditsService creditService,
                CancellationToken token) =>
            {
                try
                {
                    if (request is null) return Results.BadRequest("request is empty");
                    decimal paymentMonth = Credits.GetPaymentsMonth(request.SumCredit, request.TermMonth);
                    var credit = Credits.Create(Guid.NewGuid(), request.ClientsID, request.AccountId,
                        request.SumCredit, request.TermMonth, DateOnly.FromDateTime(DateTime.Now),
                        DateOnly.FromDateTime(DateTime.Now.AddMonths(request.TermMonth)), paymentMonth,
                        request.SumCredit, true);
                    if(!credit.IsSuccess) return Results.BadRequest(credit.Error);
                    var result = await creditService.CreateAsync(credit.Value, token);
                    return Results.Ok();
                }
                catch
                {
                    return Results.InternalServerError();
                }
            }).RequireAuthorization("OnlyForAuthUser")
            .RequireRateLimiting("GeneralPolicy");

            app.MapPost("/regDeposit", async (HttpContext context, 
                [FromBody] RegDepositRequest request,
                [FromServices] IDepositsService depositService,
                CancellationToken token) =>
            {
                try
                {
                    if (request is null) return Results.BadRequest("request is empty");
                    int percentYear = Deposits.GetCurrentPercentYear();
                    var deposit = Deposits.Create(Guid.NewGuid(), request.ClientId, request.AccountId,
                        request.SumDeposit, request.TermMonth, DateOnly.FromDateTime(DateTime.Now),
                        DateOnly.FromDateTime(DateTime.Now.AddMonths(request.TermMonth)), percentYear, true);
                    if (!deposit.IsSuccess) return Results.BadRequest(deposit.Error);
                    var result = await depositService.CreateAsync(deposit.Value, token);
                    return Results.Ok();
                }
                catch
                {
                    return Results.InternalServerError();
                }
            }).RequireAuthorization("OnlyForAuthUser")
            .RequireRateLimiting("GeneralPolicy");

            app.MapPost("/getProfile", async (HttpContext context, 
                [FromBody] GetClientRequest request, 
                [FromServices] IClientsService clientService,
                [FromServices] IAccountsService accountsService,
                [FromServices] IDepositsService depositService,
                [FromServices] ICreditsService creditService, 
                CancellationToken token) =>
            {
                try
                {
                    if (request is null) return Results.BadRequest("request is empty");
                    var client = await clientService.GetAsync(request.PassportSeries,
                        request.PassportNumber, token);
                    if (client is null) return Results.BadRequest("client is not found");
                    var idClient = client.Id;
                    if (idClient == Guid.Empty) return Results.BadRequest("client is not found");
                    var accounts = await accountsService.GetListAsync(idClient, token);
                    var deposits = await depositService.GetListAsync(idClient, token);
                    var credits = await creditService.GetListAsync(idClient, token);
                    ProfileResponce profile = new()
                    {
                        Client = client,
                        Accounts = accounts,
                        Deposits = deposits,
                        Credits = credits
                    };
                    return Results.Ok(profile);
                }
                catch
                {
                    return Results.InternalServerError();
                }
            }).RequireAuthorization("OnlyForAuthUser")
            .RequireRateLimiting("GeneralPolicy");

            app.MapDelete("/deleteAccount", async (HttpContext context, 
                [FromBody] DeleteRequest request, 
                [FromServices] IAccountsService accountsService,
                CancellationToken token) =>
            {
                try
                {
                    if (request is null) return Results.BadRequest("request is empty");
                    var result = await accountsService.DeleteAsync(request.Id, token);
                    if (result == 0) return Results.BadRequest("account not deleted");
                    return Results.Ok();
                }
                catch
                {
                    return Results.InternalServerError();
                }
            }).RequireAuthorization("OnlyForAuthUser")
            .RequireRateLimiting("GeneralPolicy");

            app.MapDelete("/deleteDeposit", async (HttpContext context, 
                [FromBody] DeleteRequest request,
                [FromServices] IDepositsService depositService,
                CancellationToken token) =>
            {
                try
                {
                    if (request is null) return Results.BadRequest("request is empty");
                    var result = await depositService.DeleteAsync(request.Id, token);
                    if (result == 0) return Results.BadRequest("deposit not deleted");
                    return Results.Ok();
                }
                catch
                {
                    return Results.InternalServerError();
                }
            }).RequireAuthorization("OnlyForAuthUser")
            .RequireRateLimiting("GeneralPolicy");

            app.MapPost("/transact", async (HttpContext context, 
                [FromBody] TransactRequest request, 
                [FromServices] ITransactionsService transactionsService,
                CancellationToken token) =>
            {
                try
                {
                    if (request is null) return Results.BadRequest("request is empty");
                    var trnsact = Transactions.Create(Guid.NewGuid(), request.ProducerAccount,
                        request.ConsumerAccount, Guid.Parse("123e4567-e89b-12d3-a456-426614174000"),
                        request.Amount, request.Description, DateOnly.FromDateTime(DateTime.Now));
                    if (!trnsact.IsSuccess) return Results.BadRequest(trnsact.Error);
                    var result = await transactionsService.CreateAsync(trnsact.Value, token);
                    return Results.Ok();
                }
                catch
                {
                    return Results.InternalServerError();
                }
            }).RequireAuthorization("OnlyForAuthUser")
            .RequireRateLimiting("GeneralPolicy");

            app.MapGet("/logout", (HttpContext context) =>
            {
                context.Response.Cookies.Delete("jwt");
                return Results.Ok();
            }).RequireAuthorization("OnlyForAuthUser")
            .RequireRateLimiting("GeneralPolicy");

            return app;
        }
    }
}
