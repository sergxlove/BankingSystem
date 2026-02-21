using BankingSystem.Extensions;
using BankingSystemApplication.Abstractions;
using BankingSystemApplication.Services;
using BankingSystemCore.Abstractions;
using BankingSystemCore.Services;
using BankingSystemDataAccess.Postgres;
using BankingSystemDataAccess.Postgres.Abstractions;
using BankingSystemDataAccess.Postgres.Infrastructure;
using BankingSystemDataAccess.Postgres.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using System.Threading.RateLimiting;

namespace BankingSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<BankingSystemDbContext>(options =>
                options.UseNpgsql("Host=localhost;Port=5432;Database=db;Username=postgres;Password=123"));
                //options.UseSqlite($"Data Source={Path.Combine(Directory.GetCurrentDirectory(), "banking.db")}"));
            builder.Services.AddScoped<ITransactionsWork, TransactionsWork>();
            builder.Services.AddScoped<IAccountsRepository, AccountsRepository>();
            builder.Services.AddScoped<IAccountsService, AccountsService>();
            builder.Services.AddScoped<IClientsRepository, ClientsRepository>();
            builder.Services.AddScoped<IClientsService, ClientsService>();
            builder.Services.AddScoped<ICreditsRepository, CreditsRepository>();
            builder.Services.AddScoped<ICreditsService, CreditsService>();
            builder.Services.AddScoped<IDepositsRepository, DepositsRepository>();
            builder.Services.AddScoped<IDepositsService, DepositsService>();
            builder.Services.AddScoped<ITransactionsRepository, TransactionsRepository>();
            builder.Services.AddScoped<ITransactionsService, TransactionsService>();
            builder.Services.AddScoped<IUsersRepository, UsersRepository>();
            builder.Services.AddScoped<IUsersService, UsersService>();
            builder.Services.AddScoped<IJwtProviderService, JwtProviderService>();
            builder.Services.AddScoped<IPasswordHasherService, PasswordHasherService>();
            builder.Services.AddScoped<ISystemTableRepository, SystemTableRepository>();
            builder.Services.AddScoped<ISystemTableService, SystemTableService>();
            builder.Services.AddScoped<ITransactMoneyService, TransactMoneyService>();
            builder.Services.AddScoped<IManagersRepository,  ManagersRepository>();
            builder.Services.AddScoped<IManagersService, ManagersService>();
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    IConfigurationSection? jwtSettings = builder.Configuration
                        .GetSection("JwtSettings");
                    options.TokenValidationParameters = new()
                    {
                        ValidateIssuer = false,
                        ValidIssuer = jwtSettings["Issuer"],
                        ValidateAudience = false,
                        ValidAudience = jwtSettings["Audience"],
                        ValidateLifetime = false,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                            .GetBytes(jwtSettings["SecretKey"]!)),
                        ValidateIssuerSigningKey = true
                    };
                    options.Events = new JwtBearerEvents()
                    {
                        OnMessageReceived = context =>
                        {
                            context.Token = context.Request.Cookies["jwt"];
                            return Task.CompletedTask;
                        }
                    };
                });
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("OnlyForAdmin", policy =>
                {
                    policy.RequireRole("admin");
                });
                options.AddPolicy("OnlyForAuthUser", policy =>
                {
                    policy.RequireRole("oper", "admin", "credit", "manager");
                });
                options.AddPolicy("OnlyForOper", policy =>
                {
                    policy.RequireRole("oper", "admin");
                });
                options.AddPolicy("OnlyForCredit", policy =>
                {
                    policy.RequireRole("credit", "admin");
                });
                options.AddPolicy("OnlyForManager", policy =>
                {
                    policy.RequireRole("manager", "admin");
                });
            });
            builder.Services.AddRateLimiter(options =>
            {
                options.AddFixedWindowLimiter("GeneralPolicy", opt =>
                {
                    opt.PermitLimit = 100;
                    opt.Window = TimeSpan.FromMinutes(1);
                    opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                    opt.QueueLimit = 10;
                });
                options.AddFixedWindowLimiter("LoginPolicy", opt =>
                {
                    opt.PermitLimit = 5;
                    opt.Window = TimeSpan.FromMinutes(1);
                });
                options.AddTokenBucketLimiter("UploadPolicy", opt =>
                {
                    opt.TokenLimit = 10;
                    opt.ReplenishmentPeriod = TimeSpan.FromMinutes(1);
                    opt.TokensPerPeriod = 2;
                    opt.AutoReplenishment = true;
                });
            });
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.SetIsOriginAllowed(origin => true)
                          .AllowAnyMethod()
                          .AllowAnyHeader()
                          .AllowCredentials();
                });
            });
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.WebHost.UseUrls("http://localhost:5001");
            var app = builder.Build();
            app.UseCors("AllowAll");
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseStaticFiles();
            app.UseRateLimiter();
            app.MapAllEndpoints();
            app.Run();
        }
    }
}
