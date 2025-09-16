using BankingSystemDataAccess.Postgres.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankingSystemDataAccess.Postgres.Configurations
{
    public class OperationsTransactionsConfigurations : IEntityTypeConfiguration<OperationsTransactionsEntity>
    {
        public void Configure(EntityTypeBuilder<OperationsTransactionsEntity> builder)
        {
            builder.HasKey(a => a.Id);
            builder.HasMany(a => a.Transactions)
                .WithOne(a => a.OperationsTransactions)
                .HasForeignKey(a => a.TypeOperation)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
