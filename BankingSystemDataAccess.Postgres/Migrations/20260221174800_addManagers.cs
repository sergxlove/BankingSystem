using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankingSystemDataAccess.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class addManagers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Managers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ClientSeries = table.Column<string>(type: "text", nullable: false),
                    ClientNumbers = table.Column<string>(type: "text", nullable: false),
                    LoginManager = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Managers", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Managers");
        }
    }
}
