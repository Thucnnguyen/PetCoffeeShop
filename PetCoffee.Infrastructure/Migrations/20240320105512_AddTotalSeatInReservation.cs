using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetCoffee.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTotalSeatInReservation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PetCoffeeProduct");

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Product",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<long>(
                name: "PetCoffeeShopId",
                table: "Product",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<int>(
                name: "BookingSeat",
                table: "Order",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Product_PetCoffeeShopId",
                table: "Product",
                column: "PetCoffeeShopId");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_PetCoffeeShop_PetCoffeeShopId",
                table: "Product",
                column: "PetCoffeeShopId",
                principalTable: "PetCoffeeShop",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_PetCoffeeShop_PetCoffeeShopId",
                table: "Product");

            migrationBuilder.DropIndex(
                name: "IX_Product_PetCoffeeShopId",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "PetCoffeeShopId",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "BookingSeat",
                table: "Order");

            migrationBuilder.CreateTable(
                name: "PetCoffeeProduct",
                columns: table => new
                {
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    PetCoffeeShopId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PetCoffeeProduct", x => new { x.ProductId, x.PetCoffeeShopId });
                    table.ForeignKey(
                        name: "FK_PetCoffeeProduct_PetCoffeeShop_PetCoffeeShopId",
                        column: x => x.PetCoffeeShopId,
                        principalTable: "PetCoffeeShop",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PetCoffeeProduct_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_PetCoffeeProduct_PetCoffeeShopId",
                table: "PetCoffeeProduct",
                column: "PetCoffeeShopId");
        }
    }
}
