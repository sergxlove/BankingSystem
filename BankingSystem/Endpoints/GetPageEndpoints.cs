namespace BankingSystem.Endpoints
{
    public static class GetPageEndpoints
    {
        public static IEndpointRouteBuilder MapGetPagesEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/", () =>
            {
                return Results.Redirect("/login");
            }).RequireRateLimiting("GeneralPolicy");

            app.MapGet("/index", async (HttpContext context) =>
            {
                context.Response.ContentType = "text/html; charset=utf-8";
                await context.Response.SendFileAsync("wwwroot/Pages/index.html");
            }).RequireAuthorization("OnlyForAuthUser")
            .RequireRateLimiting("GeneralPolicy");

            app.MapGet("/login", async (HttpContext context) =>
            {
                context.Response.ContentType = "text/html; charset=utf-8";
                await context.Response.SendFileAsync("wwwroot/Pages/login.html");
            }).RequireRateLimiting("GeneralPolicy");

            app.MapGet("/reg", async (HttpContext context) =>
            {
                context.Response.ContentType = "text/html; charset=utf-8";
                await context.Response.SendFileAsync("wwwroot/Pages/reg.html");
            }).RequireRateLimiting("GeneralPolicy");

            app.MapGet("/page/backup", async (HttpContext context) =>
            {
                context.Response.ContentType = "text/html; charset=utf-8";
                await context.Response.SendFileAsync("wwwroot/Pages/backup.html");
            }).RequireAuthorization("OnlyForAdmin")
            .RequireRateLimiting("GeneralPolicy");

            app.MapGet("/page/clients/balance", async (HttpContext context) =>
            {
                context.Response.ContentType = "text/html; charset=utf-8";
                await context.Response.SendFileAsync("wwwroot/Pages/getClientBalance.html");
            }).RequireAuthorization("OnlyForOper")
            .RequireRateLimiting("GeneralPolicy");

            app.MapGet("/page/clients/credit", async (HttpContext context) =>
            {
                context.Response.ContentType = "text/html; charset=utf-8";
                await context.Response.SendFileAsync("wwwroot/Pages/getClientCredit.html");
            }).RequireAuthorization("OnlyForOper")
            .RequireRateLimiting("GeneralPolicy");

            app.MapGet("/page/account/close", async (HttpContext context) =>
            {
                context.Response.ContentType = "text/html; charset=utf-8";
                await context.Response.SendFileAsync("wwwroot/Pages/closeAccounts.html");
            }).RequireAuthorization("OnlyForOper")
            .RequireRateLimiting("GeneralPolicy");

            app.MapGet("/page/deposit/close", async (HttpContext context) =>
            {
                context.Response.ContentType = "text/html; charset=utf-8";
                await context.Response.SendFileAsync("wwwroot/Pages/closeDeposit.html");
            }).RequireAuthorization("OnlyForOper")
            .RequireRateLimiting("GeneralPolicy");

            app.MapGet("/page/profile", async (HttpContext context) =>
            {
                context.Response.ContentType = "text/html; charset=utf-8";
                await context.Response.SendFileAsync("wwwroot/Pages/profile.html");
            }).RequireAuthorization("OnlyForAuthUser")
            .RequireRateLimiting("GeneralPolicy");

            app.MapGet("/page/account/reg", async (HttpContext context) =>
            {
                context.Response.ContentType = "text/html; charset=utf-8";
                await context.Response.SendFileAsync("wwwroot/Pages/regAccount.html");
            }).RequireAuthorization("OnlyForOper")
            .RequireRateLimiting("GeneralPolicy");

            app.MapGet("/page/client/reg", async (HttpContext context) =>
            {
                context.Response.ContentType = "text/html; charset=utf-8";
                await context.Response.SendFileAsync("wwwroot/Pages/regClient.html");
            }).RequireAuthorization("OnlyForOper")
            .RequireRateLimiting("GeneralPolicy");

            app.MapGet("/page/credit/reg", async (HttpContext context) =>
            {
                context.Response.ContentType = "text/html; charset=utf-8";
                await context.Response.SendFileAsync("wwwroot/Pages/regCredit.html");
            }).RequireAuthorization("OnlyForCredit")
            .RequireRateLimiting("GeneralPolicy");

            app.MapGet("/page/deposit/reg", async (HttpContext context) =>
            {
                context.Response.ContentType = "text/html; charset=utf-8";
                await context.Response.SendFileAsync("wwwroot/Pages/regDeposit.html");
            }).RequireAuthorization("OnlyForOper")
            .RequireRateLimiting("GeneralPolicy");

            app.MapGet("/page/transact", async (HttpContext context) =>
            {
                context.Response.ContentType = "text/html; charset=utf-8";
                await context.Response.SendFileAsync("wwwroot/Pages/transact.html");
            }).RequireAuthorization("OnlyForOper")
            .RequireRateLimiting("GeneralPolicy");

            return app;
        }
    }
}
