using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetCoffee.Infrastructure.Migrations
{
	/// <inheritdoc />
	public partial class addPackagePromotion : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "PackagePromotions",
				columns: table => new
				{
					Id = table.Column<long>(type: "bigint", nullable: false)
						.Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
					Name = table.Column<string>(type: "longtext", nullable: false)
						.Annotation("MySql:CharSet", "utf8mb4"),
					Description = table.Column<string>(type: "longtext", nullable: false)
						.Annotation("MySql:CharSet", "utf8mb4"),
					Duration = table.Column<int>(type: "int", nullable: false),
					PromotionAmount = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
					PromotionDiscount = table.Column<decimal>(type: "decimal(65,30)", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_PackagePromotions", x => x.Id);
				})
				.Annotation("MySql:CharSet", "utf8mb4");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "PackagePromotions");
		}
	}
}
