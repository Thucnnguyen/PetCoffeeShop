using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetCoffee.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddNewTableTransactionProductV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_transactionProducts_Order_ReservationId",
                table: "transactionProducts");

            migrationBuilder.DropIndex(
                name: "IX_transactionProducts_ReservationId",
                table: "transactionProducts");

            migrationBuilder.DropColumn(
                name: "ReservationId",
                table: "transactionProducts");

            migrationBuilder.RenameColumn(
                name: "ReservtionId",
                table: "transactionProducts",
                newName: "TransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_transactionProducts_TransactionId",
                table: "transactionProducts",
                column: "TransactionId");

            migrationBuilder.AddForeignKey(
                name: "FK_transactionProducts_Transaction_TransactionId",
                table: "transactionProducts",
                column: "TransactionId",
                principalTable: "Transaction",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_transactionProducts_Transaction_TransactionId",
                table: "transactionProducts");

            migrationBuilder.DropIndex(
                name: "IX_transactionProducts_TransactionId",
                table: "transactionProducts");

            migrationBuilder.RenameColumn(
                name: "TransactionId",
                table: "transactionProducts",
                newName: "ReservtionId");

            migrationBuilder.AddColumn<long>(
                name: "ReservationId",
                table: "transactionProducts",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_transactionProducts_ReservationId",
                table: "transactionProducts",
                column: "ReservationId");

            migrationBuilder.AddForeignKey(
                name: "FK_transactionProducts_Order_ReservationId",
                table: "transactionProducts",
                column: "ReservationId",
                principalTable: "Order",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
