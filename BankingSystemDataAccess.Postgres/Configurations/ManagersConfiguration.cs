using BankingSystemDataAccess.Postgres.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankingSystemDataAccess.Postgres.Configurations
{
    public class ManagersConfiguration : IEntityTypeConfiguration<ManagersEntity>
    {
        public void Configure(EntityTypeBuilder<ManagersEntity> builder)
        {
            builder.HasKey(a => a.Id);
        }
    }
}
