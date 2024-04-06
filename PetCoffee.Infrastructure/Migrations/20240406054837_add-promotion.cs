using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetCoffee.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addpromotion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Promotion",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    From = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false),
                    To = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Percent = table.Column<int>(type: "int", nullable: false),
                    PetCoffeeShopId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false),
                    CreatedById = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Promotion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Promotion_Account_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Account",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Promotion_PetCoffeeShop_PetCoffeeShopId",
                        column: x => x.PetCoffeeShopId,
                        principalTable: "PetCoffeeShop",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Promotion_CreatedById",
                table: "Promotion",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Promotion_PetCoffeeShopId",
                table: "Promotion",
                column: "PetCoffeeShopId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Promotion");
        }
    }
}
