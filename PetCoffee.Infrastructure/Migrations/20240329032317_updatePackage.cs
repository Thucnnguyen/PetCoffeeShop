using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetCoffee.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updatePackage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedAt",
                table: "PackagePromotions",
                type: "datetime(6)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "PackagePromotions");
        }
    }
}
