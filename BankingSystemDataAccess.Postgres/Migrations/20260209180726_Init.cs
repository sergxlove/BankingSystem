using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankingSystemDataAccess.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "SystemTable",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.InsertData(
                table: "SystemTable",
                columns: new[] { "Id", "NumberCardLast" },
                values: new object[] { 77, "2200100000000000" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "SystemTable",
                keyColumn: "Id",
                keyValue: 77);

            migrationBuilder.InsertData(
                table: "SystemTable",
                columns: new[] { "Id", "NumberCardLast" },
                values: new object[] { 1, "2200100000000000" });
        }
    }
}
