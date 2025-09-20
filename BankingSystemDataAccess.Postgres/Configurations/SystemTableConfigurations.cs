using BankingSystemDataAccess.Postgres.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankingSystemDataAccess.Postgres.Configurations
{
    public class SystemTableConfigurations : IEntityTypeConfiguration<SystemTableEntity>
    {
        public void Configure(EntityTypeBuilder<SystemTableEntity> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasData(new SystemTableEntity() { Id = 1, NumberCardLast = "2200100000000000" });
        }
    }
}
