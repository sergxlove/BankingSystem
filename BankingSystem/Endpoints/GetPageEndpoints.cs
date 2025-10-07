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
            return app;
        }
    }
}
