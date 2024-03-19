using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetCoffee.Infrastructure.Migrations
{
	/// <inheritdoc />
	public partial class updateArea : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropColumn(
				name: "TotalSeatBook",
				table: "Order");

			migrationBuilder.AddColumn<long>(
				name: "PricePerHour",
				table: "Area",
				type: "bigint",
				nullable: false,
				defaultValue: 0L);

			migrationBuilder.AddColumn<int>(
				name: "TotalSeat",
				table: "Area",
				type: "int",
				nullable: false,
				defaultValue: 0);
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropColumn(
				name: "PricePerHour",
				table: "Area");

			migrationBuilder.DropColumn(
				name: "TotalSeat",
				table: "Area");

			migrationBuilder.AddColumn<int>(
				name: "TotalSeatBook",
				table: "Order",
				type: "int",
				nullable: false,
				defaultValue: 0);
		}
	}
}
