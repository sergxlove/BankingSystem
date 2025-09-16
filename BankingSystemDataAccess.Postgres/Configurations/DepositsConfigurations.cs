using BankingSystemDataAccess.Postgres.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankingSystemDataAccess.Postgres.Configurations
{
    public class DepositsConfigurations : IEntityTypeConfiguration<DepositsEntity>
    {
        public void Configure(EntityTypeBuilder<DepositsEntity> builder)
        {
            builder.HasKey(a => a.Id);
            builder.HasOne(a => a.Clients)
                .WithMany(a => a.Deposits)
                .HasForeignKey(a => a.ClientId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(a => a.Accounts)
                .WithMany(a => a.Deposits)
                .HasForeignKey(a => a.AccountId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
