using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetCoffee.Infrastructure.Migrations
{
	/// <inheritdoc />
	public partial class updateRatePetField : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AlterColumn<string>(
				name: "Comment",
				table: "RatePet",
				type: "longtext",
				nullable: true,
				oldClrType: typeof(long),
				oldType: "bigint")
				.Annotation("MySql:CharSet", "utf8mb4");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AlterColumn<long>(
				name: "Comment",
				table: "RatePet",
				type: "bigint",
				nullable: false,
				defaultValue: 0L,
				oldClrType: typeof(string),
				oldType: "longtext",
				oldNullable: true)
				.OldAnnotation("MySql:CharSet", "utf8mb4");
		}
	}
}
