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
            builder.HasData(new OperationsTransactionsEntity
            {
                Id = Guid.Parse("123e4567-e89b-12d3-a456-426614174000"),
                TypeOperation = "Transfer",
                Description = ""
            });
        }
    }
}
