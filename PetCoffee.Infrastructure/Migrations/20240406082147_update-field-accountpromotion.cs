using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetCoffee.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updatefieldaccountpromotion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountPromotion_PetCoffeeShop_ShopId",
                table: "AccountPromotion");

            migrationBuilder.RenameColumn(
                name: "ShopId",
                table: "AccountPromotion",
                newName: "PromotionId");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountPromotion_Promotion_PromotionId",
                table: "AccountPromotion",
                column: "PromotionId",
                principalTable: "Promotion",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountPromotion_Promotion_PromotionId",
                table: "AccountPromotion");

            migrationBuilder.RenameColumn(
                name: "PromotionId",
                table: "AccountPromotion",
                newName: "ShopId");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountPromotion_PetCoffeeShop_ShopId",
                table: "AccountPromotion",
                column: "ShopId",
                principalTable: "PetCoffeeShop",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
