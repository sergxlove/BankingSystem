using Microsoft.EntityFrameworkCore;

namespace BankingSystemDataAccess.Postgres
{
    public class BankingSystemDbContext : DbContext
    {

        public BankingSystemDbContext(DbContextOptions<BankingSystemDbContext> options) 
            : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
