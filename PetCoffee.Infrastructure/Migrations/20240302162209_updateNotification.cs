using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetCoffee.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateNotification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ReservationId",
                table: "Transaction",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Notification",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_ReservationId",
                table: "Transaction",
                column: "ReservationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_Order_ReservationId",
                table: "Transaction",
                column: "ReservationId",
                principalTable: "Order",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Order_ReservationId",
                table: "Transaction");

            migrationBuilder.DropIndex(
                name: "IX_Transaction_ReservationId",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "ReservationId",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Notification");
        }
    }
}
