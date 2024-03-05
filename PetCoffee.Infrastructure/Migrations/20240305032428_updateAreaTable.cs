using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetCoffee.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateAreaTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Area",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "CreatedById",
                table: "Area",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Area",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Area",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_Area_CreatedById",
                table: "Area",
                column: "CreatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Area_Account_CreatedById",
                table: "Area",
                column: "CreatedById",
                principalTable: "Account",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Area_Account_CreatedById",
                table: "Area");

            migrationBuilder.DropIndex(
                name: "IX_Area_CreatedById",
                table: "Area");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Area");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Area");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Area");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Area");
        }
    }
}
