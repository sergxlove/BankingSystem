using BankingSystemDataAccess.Postgres.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankingSystemDataAccess.Postgres.Configurations
{
    public class ClientsConfigurations : IEntityTypeConfiguration<ClientsEntity>
    {
        public void Configure(EntityTypeBuilder<ClientsEntity> builder)
        {
            builder.HasKey(a => a.Id);
            builder.HasMany(a => a.Credits)
                .WithOne(a => a.Clients)
                .HasForeignKey(a => a.ClientId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasMany(a => a.Deposits)
                .WithOne(a => a.Clients)
                .HasForeignKey(a => a.ClientId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasMany(a => a.Accounts)
                .WithOne(a => a.Clients)
                .HasForeignKey(a => a.ClientsId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
