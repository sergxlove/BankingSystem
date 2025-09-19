using BankingSystemDataAccess.Postgres.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankingSystemDataAccess.Postgres.Configurations
{

    public class UsersConfigurations : IEntityTypeConfiguration<UsersEntity>
    {
        public void Configure(EntityTypeBuilder<UsersEntity> builder)
        {
            builder.HasKey(a => a.Id);
        }
    }
}
