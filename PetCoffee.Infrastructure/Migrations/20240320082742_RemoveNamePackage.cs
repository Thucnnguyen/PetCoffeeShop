using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetCoffee.Infrastructure.Migrations
{
	/// <inheritdoc />
	public partial class RemoveNamePackage : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropColumn(
				name: "Name",
				table: "PackagePromotions");

			migrationBuilder.AlterColumn<string>(
				name: "Description",
				table: "PackagePromotions",
				type: "longtext",
				nullable: true,
				oldClrType: typeof(string),
				oldType: "longtext")
				.Annotation("MySql:CharSet", "utf8mb4")
				.OldAnnotation("MySql:CharSet", "utf8mb4");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.UpdateData(
				table: "PackagePromotions",
				keyColumn: "Description",
				keyValue: null,
				column: "Description",
				value: "");

			migrationBuilder.AlterColumn<string>(
				name: "Description",
				table: "PackagePromotions",
				type: "longtext",
				nullable: false,
				oldClrType: typeof(string),
				oldType: "longtext",
				oldNullable: true)
				.Annotation("MySql:CharSet", "utf8mb4")
				.OldAnnotation("MySql:CharSet", "utf8mb4");

			migrationBuilder.AddColumn<string>(
				name: "Name",
				table: "PackagePromotions",
				type: "longtext",
				nullable: false)
				.Annotation("MySql:CharSet", "utf8mb4");
		}
	}
}
