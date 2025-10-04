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
                options.UseNpgsql("User ID=postgres;Password=123;Host=localhost;Port=5432;Database=db;"));
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
                    policy.RequireClaim(ClaimTypes.Role, "admin");
                });
                options.AddPolicy("OnlyForAuthUser", policy =>
                {
                    policy.RequireClaim(ClaimTypes.Role, "user");
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
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            var app = builder.Build();
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseStaticFiles();
            app.UseRateLimiter();
            app.MapAllEndpoints();
            app.Run();
        }
    }
}
