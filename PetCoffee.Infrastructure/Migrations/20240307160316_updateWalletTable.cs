using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetCoffee.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateWalletTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Wallet_Account_AccountId",
                table: "Wallet");

            migrationBuilder.DropIndex(
                name: "IX_Wallet_AccountId",
                table: "Wallet");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "Wallet");

            migrationBuilder.AddColumn<string>(
                name: "ReferenceTransactionId",
                table: "Transaction",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "Transaction",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReferenceTransactionId",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "Url",
                table: "Transaction");

            migrationBuilder.AddColumn<long>(
                name: "AccountId",
                table: "Wallet",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Wallet_AccountId",
                table: "Wallet",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Wallet_Account_AccountId",
                table: "Wallet",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
