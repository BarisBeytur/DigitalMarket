using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitalMarket.Data.Migrations
{
    /// <inheritdoc />
    public partial class mig_user : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_AspNetUsers_ApplicationUserId",
                schema: "dbo",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_User_UserId",
                schema: "dbo",
                table: "Order");

            migrationBuilder.DropTable(
                name: "User",
                schema: "dbo");

            migrationBuilder.DropIndex(
                name: "IX_Order_ApplicationUserId",
                schema: "dbo",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_DigitalWalletId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                schema: "dbo",
                table: "Order");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_DigitalWalletId",
                table: "AspNetUsers",
                column: "DigitalWalletId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_AspNetUsers_UserId",
                schema: "dbo",
                table: "Order",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_AspNetUsers_UserId",
                schema: "dbo",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_DigitalWalletId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<long>(
                name: "ApplicationUserId",
                schema: "dbo",
                table: "Order",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "User",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DigitalWalletId = table.Column<long>(type: "bigint", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    InsertDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InsertUser = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Role = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                    table.ForeignKey(
                        name: "FK_User_DigitalWallet_DigitalWalletId",
                        column: x => x.DigitalWalletId,
                        principalSchema: "dbo",
                        principalTable: "DigitalWallet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Order_ApplicationUserId",
                schema: "dbo",
                table: "Order",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_DigitalWalletId",
                table: "AspNetUsers",
                column: "DigitalWalletId");

            migrationBuilder.CreateIndex(
                name: "IX_User_DigitalWalletId",
                schema: "dbo",
                table: "User",
                column: "DigitalWalletId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_Email",
                schema: "dbo",
                table: "User",
                column: "Email",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_AspNetUsers_ApplicationUserId",
                schema: "dbo",
                table: "Order",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_User_UserId",
                schema: "dbo",
                table: "Order",
                column: "UserId",
                principalSchema: "dbo",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
