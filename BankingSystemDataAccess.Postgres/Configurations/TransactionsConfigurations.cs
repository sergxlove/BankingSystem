using BankingSystemDataAccess.Postgres.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankingSystemDataAccess.Postgres.Configurations
{
    public class TransactionsConfigurations : IEntityTypeConfiguration<TransactionsEntity>
    {
        public void Configure(EntityTypeBuilder<TransactionsEntity> builder)
        {
            builder.HasKey(a => a.Id);
            builder.HasOne(a => a.OperationsTransactions)
                .WithMany(a => a.Transactions)
                .HasForeignKey(a => a.TypeOperation)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
