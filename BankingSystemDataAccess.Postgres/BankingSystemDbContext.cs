using BankingSystemDataAccess.Postgres.Configurations;
using BankingSystemDataAccess.Postgres.Models;
using Microsoft.EntityFrameworkCore;

namespace BankingSystemDataAccess.Postgres
{
    public class BankingSystemDbContext : DbContext
    {
        public BankingSystemDbContext(DbContextOptions<BankingSystemDbContext> options) 
            : base(options) 
        {
            Database.EnsureCreated();
        }

        public DbSet<AccountsEntity> Accounts { get; set; }
        public DbSet<ClientsEntity> Clients { get; set; }
        public DbSet<CreditsEntity> Credits { get; set; }
        public DbSet<DepositsEntity> Deposits { get; set; }
        public DbSet<OperationsTransactionsEntity> OperationsTransactions { get; set; }
        public DbSet<TransactionsEntity> Transactions { get; set; }
        public DbSet<UsersEntity> Users { get; set; }
        public DbSet<SystemTableEntity> SystemTable { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AccountsConfigurations());
            modelBuilder.ApplyConfiguration(new ClientsConfigurations());
            modelBuilder.ApplyConfiguration(new CreditsConfigurations());
            modelBuilder.ApplyConfiguration(new DepositsConfigurations());
            modelBuilder.ApplyConfiguration(new OperationsTransactionsConfigurations());
            modelBuilder.ApplyConfiguration(new TransactionsConfigurations());
            modelBuilder.ApplyConfiguration(new UsersConfigurations());
            modelBuilder.ApplyConfiguration(new SystemTableConfigurations());
            base.OnModelCreating(modelBuilder);
        }
    }
}
