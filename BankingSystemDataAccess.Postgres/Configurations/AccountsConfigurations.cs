using BankingSystemDataAccess.Postgres.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankingSystemDataAccess.Postgres.Configurations
{
    public class AccountsConfigurations : IEntityTypeConfiguration<AccountsEntity>
    {
        public void Configure(EntityTypeBuilder<AccountsEntity> builder)
        {
            builder.HasKey(a => a.Id);
            builder.HasOne(a => a.Clients)
                .WithMany(a => a.Accounts)
                .HasForeignKey(a => a.ClientsId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasMany(a => a.Credits)
                .WithOne(a => a.Accounts)
                .HasForeignKey(a => a.AccountId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasMany(a => a.Deposits)
                .WithOne(a => a.Accounts)
                .HasForeignKey(a => a.AccountId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
