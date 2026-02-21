using BankingSystem.Requests;
using BankingSystem.Response;
using BankingSystemApplication.Abstractions;
using BankingSystemApplication.Requests;
using BankingSystemCore.Abstractions;
using BankingSystemCore.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using System.Text;

namespace BankingSystem.Endpoints
{
    public static class BankingOperationsEndpoints
    {
        public static IEndpointRouteBuilder MapBankingOperationsEndpoints(this IEndpointRouteBuilder app)
        {
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
                    string userRole = await userService.GetRoleAsync(request.Username, token);
                    var claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.Role, userRole),
                        new Claim(ClaimTypes.Email, request.Username),
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
                    if(await userService.CheckAsync(user.Value.Username, token))
                    {
                        return Results.BadRequest("Данный пользователь уже есть");
                    }
                    var result = await userService.CreateAsync(user.Value, token);
                    var claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.Role, "user"),
                        new Claim(ClaimTypes.Email, request.Username),
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
                    if(await clientService.CheckAsync(client.Value.PassportSeries, 
                        client.Value.PassportNumber, token))
                    {
                        return Results.BadRequest("Данный кллиент уже существует");
                    }
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

            app.MapPost("/getShortClient", async (HttpContext context,
                [FromBody] GetClientRequest request,
                [FromServices] IClientsService clientService,
                CancellationToken token) =>
            {
                try
                {
                    if (request.PassportSeries == string.Empty || request.PassportNumber == string.Empty)
                        return Results.BadRequest("passport data is empty");
                    var client = await clientService.GetAsync(request.PassportSeries,
                        request.PassportNumber, token);
                    if (client is null) return Results.BadRequest("client is not found");
                    ShortClientResponse shortClient = new()
                    {
                        Id = client.Id,
                        Name = client.FirstName + " " + client.SecondName + " " + client.LastName,
                        DateBirth = client.BirthDate
                    };
                    return Results.Ok(shortClient);
                }
                catch
                {
                    return Results.InternalServerError();
                }
            }).RequireAuthorization("OnlyForAuthUser")
            .RequireRateLimiting("GeneralPolicy");

            app.MapPost("/linkManager", async (HttpContext context,
                [FromBody] ManagerRequest request, 
                [FromServices] IClientsService clientsService, 
                [FromServices] IManagersService managerService,
                CancellationToken token) =>
            {
                try
                {
                    if (request.PassportSeries == string.Empty || request.PassportNumber == string.Empty
                        || request.LoginManager == string.Empty)
                        return Results.BadRequest("data is empty");
                    var manager = Managers.Create(Guid.NewGuid(), request.PassportSeries,
                        request.PassportNumber, request.LoginManager);
                    if(!string.IsNullOrEmpty(manager.Error)) return Results.BadRequest(manager.Error);
                    await managerService.AddAsync(manager.Value, token);
                    return Results.Ok();
                }
                catch
                {
                    return Results.InternalServerError();
                }
            }).RequireAuthorization("OnlyForAuthUser")
            .RequireRateLimiting("GeneralPolicy");


            app.MapPost("/getFullClient", async (HttpContext context, 
                [FromBody] GetClientRequest request, 
                [FromServices] IClientsService clientService, 
                CancellationToken token) =>
            {
                if (request.PassportSeries == string.Empty || request.PassportNumber == string.Empty)
                    return Results.BadRequest("passport data is empty");
                var client = await clientService.GetAsync(request.PassportSeries,
                    request.PassportNumber, token);
                if (client is null) return Results.BadRequest("client is not found");
                return Results.Ok(client);

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

            app.MapPost("/getAccount", async (HttpContext context,
                [FromBody] IdRequest request, 
                [FromServices] IAccountsService accountsService, 
                CancellationToken token) =>
            {
                try
                {
                    if (request is null) return Results.BadRequest("request is empty");
                    var account = await accountsService.GetAsync(request.Id, token);
                    if (account is null) return Results.BadRequest("no found account");
                    return Results.Ok(account);
                }
                catch
                {
                    return Results.InternalServerError();
                }
            });

            app.MapPost("/getShortAccounts", async (HttpContext context,
                [FromBody] IdRequest request,
                [FromServices] IAccountsService accountService,
                CancellationToken token) =>
            {
                try
                {
                    if (request is null) return Results.BadRequest("request is empty");
                    var accounts = await accountService.GetListAsync(request.Id, token);
                    List<ShortAccountsResponse> shortAccounts = new();
                    foreach (var account in accounts) 
                    {
                        shortAccounts.Add(new()
                        {
                            Id = account.Id,
                            Number = account.AccountNumber,
                            TypeAccount = account.AccountType,
                            Currency = account.CurrencyCode,
                            Balance = account.Balance,
                        });
                    }
                    return Results.Ok(shortAccounts);
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

            app.MapPost("/getDeposit", async (HttpContext context,
                [FromBody] IdRequest request,
                [FromServices] IDepositsService depositService,
                CancellationToken token) =>
            {
                try
                {
                    if (request is null) return Results.BadRequest("request is empty");
                    var deposit = await depositService.GetAsync(request.Id, token);
                    if (deposit is null) return Results.BadRequest("no found deposit");
                    return Results.Ok(deposit);
                }
                catch
                {
                    return Results.InternalServerError();
                }
            });

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
                    ProfileResponse profile = new()
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
                [FromBody] IdRequest request, 
                [FromServices] IAccountsService accountsService,
                CancellationToken token) =>
            {
                try
                {
                    if (request is null) return Results.BadRequest("request is empty");
                    var result = await accountsService.DeleteAsync(request.Id, token);
                    if (result == 0) return Results.BadRequest("Счет не был удален, так как он либо отсутствует, либо баланс не равен 0");
                    return Results.Ok();
                }
                catch
                {
                    return Results.InternalServerError();
                }
            }).RequireAuthorization("OnlyForAuthUser")
            .RequireRateLimiting("GeneralPolicy");

            app.MapDelete("/deleteDeposit", async (HttpContext context, 
                [FromBody] IdRequest request,
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
                [FromServices] ITransactMoneyService transactionMoneyService,
                CancellationToken token) =>
            {
                try
                {
                    if (request is null) return Results.BadRequest("request is empty");
                    var trnsact = Transactions.Create(Guid.NewGuid(), request.ProducerAccount,
                        request.ConsumerAccount, Guid.Parse("123e4567-e89b-12d3-a456-426614174000"),
                        request.Amount, request.Description, DateOnly.FromDateTime(DateTime.Now));
                    if (!trnsact.IsSuccess) return Results.BadRequest(trnsact.Error);
                    var result = await transactionMoneyService.ExecuteTransactAsync(trnsact.Value, token);
                    if (result != string.Empty) return Results.BadRequest(result);
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

            app.MapGet("/api/backup/create", async () =>
            {
                string fileName = $"backup_{DateTime.Now:yyyyMMdd_HHmmss}.sql";
                string containerName = "postgres-container";
                try
                {
                    var process = new Process
                    {
                        StartInfo = new ProcessStartInfo
                        {
                            FileName = "docker",
                            Arguments = $"exec {containerName} pg_dump -U postgres -d db",
                            RedirectStandardOutput = true,
                            RedirectStandardError = true,
                            UseShellExecute = false,
                            CreateNoWindow = true,
                            StandardOutputEncoding = Encoding.UTF8
                        }
                    };
                    process.Start();
                    string output = await process.StandardOutput.ReadToEndAsync();
                    string error = await process.StandardError.ReadToEndAsync();
                    await process.WaitForExitAsync();
                    if (process.ExitCode != 0)
                    {
                        return Results.BadRequest($"Ошибка pg_dump: {error}");
                    }
                    var fileBytes = Encoding.UTF8.GetBytes(output);
                    return Results.File(fileBytes, "application/octet-stream", fileName);
                }
                catch (Exception ex)
                {
                    return Results.BadRequest($"Ошибка: {ex.Message}");
                }
            }).RequireAuthorization("OnlyForAuthUser");

            app.MapPost("/api/clients/balance-range", async (HttpContext context,
                [FromBody] BalanceRequest request,
                [FromServices] IClientsService clientService,
                CancellationToken token) =>
            {
                if (request.MinBalance < 0 || request.MaxBalance < 0)
                {
                    return Results.BadRequest("Баланс не может быть отрицательным");
                }

                if (request.MinBalance > request.MaxBalance)
                {
                    return Results.BadRequest("Минимальный баланс не может быть больше максимального");
                }

                var result = await clientService.GetClientsByBalanceRangeAsync(request.MinBalance,
                    request.MaxBalance, token);

                if (result == null || result.Count == 0)
                {
                    return Results.NotFound("Клиенты в указанном диапазоне не найдены");
                }

                return Results.Ok(result);
            }).RequireAuthorization("OnlyForAuthUser");

            app.MapPost("/api/credits/borrowers-by-months", async (HttpContext context,
                [FromBody] MonthsLeftRequest request,
                [FromServices] IClientsService clientService,
                CancellationToken token) =>
            {
                if (request.MaxMonthsLeft <= 0)
                {
                    return Results.BadRequest("Количество месяцев должно быть больше 0");
                }
                var result = await clientService.GetBorrowersByMonthsLeftAsync(request.MaxMonthsLeft, token);
                if (result == null || result.Count == 0)
                {
                    return Results.NotFound("Заемщики с указанным сроком не найдены");
                }

                return Results.Ok(result);
            }).RequireAuthorization("OnlyForAuthUser");

            return app;
        }
    }
}
