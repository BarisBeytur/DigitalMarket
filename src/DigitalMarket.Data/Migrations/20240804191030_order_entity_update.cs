using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitalMarket.Data.Migrations
{
    /// <inheritdoc />
    public partial class order_entity_update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<short>(
                name: "Status",
                schema: "dbo",
                table: "Order",
                type: "smallint",
                nullable: false,
                defaultValue: (short)1);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                schema: "dbo",
                table: "Order");
        }
    }
}
