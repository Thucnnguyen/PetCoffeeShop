using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetCoffee.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Account_Account_CreatedById",
                table: "Account");

            migrationBuilder.DropIndex(
                name: "IX_Account_CreatedById",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Account");

            migrationBuilder.RenameColumn(
                name: "StartTime",
                table: "PetCoffeeShop",
                newName: "OpeningTime");

            migrationBuilder.RenameColumn(
                name: "EndTime",
                table: "PetCoffeeShop",
                newName: "ClosedTime");

            migrationBuilder.AddColumn<int>(
                name: "ParkingType",
                table: "PetCoffeeShop",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Spayed",
                table: "Pet",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Gender",
                table: "Account",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ParkingType",
                table: "PetCoffeeShop");

            migrationBuilder.DropColumn(
                name: "Spayed",
                table: "Pet");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "Account");

            migrationBuilder.RenameColumn(
                name: "OpeningTime",
                table: "PetCoffeeShop",
                newName: "StartTime");

            migrationBuilder.RenameColumn(
                name: "ClosedTime",
                table: "PetCoffeeShop",
                newName: "EndTime");

            migrationBuilder.AddColumn<long>(
                name: "CreatedById",
                table: "Account",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Account_CreatedById",
                table: "Account",
                column: "CreatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Account_Account_CreatedById",
                table: "Account",
                column: "CreatedById",
                principalTable: "Account",
                principalColumn: "Id");
        }
    }
}
