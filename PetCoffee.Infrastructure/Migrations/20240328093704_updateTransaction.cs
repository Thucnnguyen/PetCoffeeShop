using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetCoffee.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateTransaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "PackagePromotionId",
                table: "Transaction",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "PetCoffeeShopId",
                table: "Transaction",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_PackagePromotionId",
                table: "Transaction",
                column: "PackagePromotionId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_PetCoffeeShopId",
                table: "Transaction",
                column: "PetCoffeeShopId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_PackagePromotions_PackagePromotionId",
                table: "Transaction",
                column: "PackagePromotionId",
                principalTable: "PackagePromotions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_PetCoffeeShop_PetCoffeeShopId",
                table: "Transaction",
                column: "PetCoffeeShopId",
                principalTable: "PetCoffeeShop",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_PackagePromotions_PackagePromotionId",
                table: "Transaction");

            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_PetCoffeeShop_PetCoffeeShopId",
                table: "Transaction");

            migrationBuilder.DropIndex(
                name: "IX_Transaction_PackagePromotionId",
                table: "Transaction");

            migrationBuilder.DropIndex(
                name: "IX_Transaction_PetCoffeeShopId",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "PackagePromotionId",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "PetCoffeeShopId",
                table: "Transaction");
        }
    }
}
