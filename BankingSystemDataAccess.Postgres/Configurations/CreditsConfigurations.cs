using BankingSystemDataAccess.Postgres.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankingSystemDataAccess.Postgres.Configurations
{
    public class CreditsConfigurations : IEntityTypeConfiguration<CreditsEntity>
    {
        public void Configure(EntityTypeBuilder<CreditsEntity> builder)
        {
            builder.HasKey(a => a.Id);
            builder.HasOne(a => a.Clients)
                .WithMany(a => a.Credits)
                .HasForeignKey(a => a.ClientId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(a => a.Accounts)
                .WithMany(a => a.Credits)
                .HasForeignKey(a => a.AccountId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
