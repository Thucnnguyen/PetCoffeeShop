using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetCoffee.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addnotnullableareaareaidatreservation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_Area_AreaId",
                table: "Order");

            migrationBuilder.AlterColumn<long>(
                name: "AreaId",
                table: "Order",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Area_AreaId",
                table: "Order",
                column: "AreaId",
                principalTable: "Area",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_Area_AreaId",
                table: "Order");

            migrationBuilder.AlterColumn<long>(
                name: "AreaId",
                table: "Order",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Area_AreaId",
                table: "Order",
                column: "AreaId",
                principalTable: "Area",
                principalColumn: "Id");
        }
    }
}
