using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetCoffee.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateDatabaseForAccountandShop : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Account_PetCoffeeShop_PetCoffeeShopId",
                table: "Account");

            migrationBuilder.DropIndex(
                name: "IX_Account_PetCoffeeShopId",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "PetCoffeeShopId",
                table: "Account");

            migrationBuilder.AddColumn<long>(
                name: "ShopId",
                table: "Post",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ShopId",
                table: "Comment",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AccountShop",
                columns: table => new
                {
                    AccountId = table.Column<long>(type: "bigint", nullable: false),
                    ShopId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountShop", x => new { x.AccountId, x.ShopId });
                    table.ForeignKey(
                        name: "FK_AccountShop_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccountShop_PetCoffeeShop_ShopId",
                        column: x => x.ShopId,
                        principalTable: "PetCoffeeShop",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Post_ShopId",
                table: "Post",
                column: "ShopId");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_ShopId",
                table: "Comment",
                column: "ShopId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountShop_ShopId",
                table: "AccountShop",
                column: "ShopId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_PetCoffeeShop_ShopId",
                table: "Comment",
                column: "ShopId",
                principalTable: "PetCoffeeShop",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Post_PetCoffeeShop_ShopId",
                table: "Post",
                column: "ShopId",
                principalTable: "PetCoffeeShop",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_PetCoffeeShop_ShopId",
                table: "Comment");

            migrationBuilder.DropForeignKey(
                name: "FK_Post_PetCoffeeShop_ShopId",
                table: "Post");

            migrationBuilder.DropTable(
                name: "AccountShop");

            migrationBuilder.DropIndex(
                name: "IX_Post_ShopId",
                table: "Post");

            migrationBuilder.DropIndex(
                name: "IX_Comment_ShopId",
                table: "Comment");

            migrationBuilder.DropColumn(
                name: "ShopId",
                table: "Post");

            migrationBuilder.DropColumn(
                name: "ShopId",
                table: "Comment");

            migrationBuilder.AddColumn<long>(
                name: "PetCoffeeShopId",
                table: "Account",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Account_PetCoffeeShopId",
                table: "Account",
                column: "PetCoffeeShopId");

            migrationBuilder.AddForeignKey(
                name: "FK_Account_PetCoffeeShop_PetCoffeeShopId",
                table: "Account",
                column: "PetCoffeeShopId",
                principalTable: "PetCoffeeShop",
                principalColumn: "Id");
        }
    }
}
