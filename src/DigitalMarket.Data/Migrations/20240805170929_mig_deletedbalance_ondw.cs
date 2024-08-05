using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitalMarket.Data.Migrations
{
    /// <inheritdoc />
    public partial class mig_deletedbalance_ondw : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Balance",
                schema: "dbo",
                table: "DigitalWallet");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Balance",
                schema: "dbo",
                table: "DigitalWallet",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);
        }
    }
}
