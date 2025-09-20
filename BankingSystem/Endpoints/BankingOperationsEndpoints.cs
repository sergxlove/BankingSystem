using BankingSystem.Requests;
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
            });

            app.MapGet("/index", async (HttpContext context) =>
            {
                context.Response.ContentType = "text/html; charset=utf-8";
                await context.Response.SendFileAsync("wwwroot/index.html");
            }).RequireAuthorization("OnlyForAuthUser");

            app.MapGet("/login", async (HttpContext context) =>
            {
                context.Response.ContentType = "text/html; charset=utf-8";
                await context.Response.SendFileAsync("wwwroot/login.html");
            });

            app.MapGet("/reg", async (HttpContext context) =>
            {
                context.Response.ContentType = "text/html; charset=utf-8";
                await context.Response.SendFileAsync("wwwroot/login.html");
            });

            app.MapPost("/login", async (HttpContext context,
                [FromBody] LoginRequest request,
                [FromServices] IUsersService userService,
                [FromServices] IJwtProviderService jwtService,
                CancellationToken token) =>
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
            });

            app.MapPost("/reg", async (HttpContext context,
                [FromBody] RegistrRequest request,
                [FromServices] IUsersService userService,
                [FromServices] IJwtProviderService jwtService,
                [FromServices] IPasswordHasherService passwordHasher,
                CancellationToken token) =>
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
            });

            app.MapPost("/regClient", async (HttpContext context, 
                [FromBody] ClientsRequest request,
                [FromServices] IClientsService clientService, 
                CancellationToken token) =>
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
            }).RequireAuthorization("OnlyForAuthUser");

            app.MapPost("/getClient", async (HttpContext context,
                [FromBody] GetClientRequest request,
                [FromServices] IClientsService clientService,
                CancellationToken token) =>
            {
                if (request.PassportSeries == string.Empty || request.PassportNumber == string.Empty)
                    return Results.BadRequest("passport data is empty");
                var idClient = await clientService.GetIdAsync(request.PassportSeries, 
                    request.PassportNumber, token);
                if (idClient == Guid.Empty) return Results.BadRequest("client is not found");
                return Results.Ok(idClient);
            }).RequireAuthorization("OnlyForAuthUser");

            app.MapPost("/regAccount", async (HttpContext context, 
                [FromBody] RegAccountRequest request, 
                [FromServices] IAccountsService accountsService,
                CancellationToken token) =>
            {
                if (request is null) return Results.BadRequest("request is empty");
                string numberCard = 
                var acccount = Accounts.Create(Guid.NewGuid(), request.ClientsId, request.AccountType)

            }).RequireAuthorization("OnlyForAuthUser");

            app.MapGet("/logout", (HttpContext context) =>
            {
                context.Response.Cookies.Delete("jwt");
                return Results.Ok();
            }).RequireAuthorization("OnlyForAuthUser");

            return app;
        }
    }
}
