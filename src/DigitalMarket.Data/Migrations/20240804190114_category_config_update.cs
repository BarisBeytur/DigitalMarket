using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitalMarket.Data.Migrations
{
    /// <inheritdoc />
    public partial class category_config_update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductCategory_Category_CategoryId",
                schema: "dbo",
                table: "ProductCategory");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCategory_Category_CategoryId",
                schema: "dbo",
                table: "ProductCategory",
                column: "CategoryId",
                principalSchema: "dbo",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductCategory_Category_CategoryId",
                schema: "dbo",
                table: "ProductCategory");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCategory_Category_CategoryId",
                schema: "dbo",
                table: "ProductCategory",
                column: "CategoryId",
                principalSchema: "dbo",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
