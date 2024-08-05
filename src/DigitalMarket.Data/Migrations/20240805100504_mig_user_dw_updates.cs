using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitalMarket.Data.Migrations
{
    /// <inheritdoc />
    public partial class mig_user_dw_updates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_DigitalWallet_DigitalWalletId",
                schema: "dbo",
                table: "User");

            migrationBuilder.CreateIndex(
                name: "IX_User_Email",
                schema: "dbo",
                table: "User",
                column: "Email",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_User_DigitalWallet_DigitalWalletId",
                schema: "dbo",
                table: "User",
                column: "DigitalWalletId",
                principalSchema: "dbo",
                principalTable: "DigitalWallet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_DigitalWallet_DigitalWalletId",
                schema: "dbo",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_Email",
                schema: "dbo",
                table: "User");

            migrationBuilder.AddForeignKey(
                name: "FK_User_DigitalWallet_DigitalWalletId",
                schema: "dbo",
                table: "User",
                column: "DigitalWalletId",
                principalSchema: "dbo",
                principalTable: "DigitalWallet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
