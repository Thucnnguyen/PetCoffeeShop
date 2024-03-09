using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetCoffee.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateTransactionTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "PetId",
                table: "Transaction",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_PetId",
                table: "Transaction",
                column: "PetId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_Pet_PetId",
                table: "Transaction",
                column: "PetId",
                principalTable: "Pet",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Pet_PetId",
                table: "Transaction");

            migrationBuilder.DropIndex(
                name: "IX_Transaction_PetId",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "PetId",
                table: "Transaction");
        }
    }
}
