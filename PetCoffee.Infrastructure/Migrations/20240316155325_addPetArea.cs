using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetCoffee.Infrastructure.Migrations
{
	/// <inheritdoc />
	public partial class addPetArea : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropForeignKey(
				name: "FK_Pet_Area_AreaId",
				table: "Pet");

			migrationBuilder.DropIndex(
				name: "IX_Pet_AreaId",
				table: "Pet");

			migrationBuilder.DropColumn(
				name: "AreaId",
				table: "Pet");

			migrationBuilder.CreateTable(
				name: "PetArea",
				columns: table => new
				{
					Id = table.Column<long>(type: "bigint", nullable: false)
						.Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
					PetId = table.Column<long>(type: "bigint", nullable: false),
					AreaId = table.Column<long>(type: "bigint", nullable: false),
					StartTime = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false),
					EndTime = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_PetArea", x => x.Id);
					table.ForeignKey(
						name: "FK_PetArea_Area_AreaId",
						column: x => x.AreaId,
						principalTable: "Area",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
					table.ForeignKey(
						name: "FK_PetArea_Pet_PetId",
						column: x => x.PetId,
						principalTable: "Pet",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				})
				.Annotation("MySql:CharSet", "utf8mb4");

			migrationBuilder.CreateIndex(
				name: "IX_PetArea_AreaId",
				table: "PetArea",
				column: "AreaId");

			migrationBuilder.CreateIndex(
				name: "IX_PetArea_PetId",
				table: "PetArea",
				column: "PetId");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "PetArea");

			migrationBuilder.AddColumn<long>(
				name: "AreaId",
				table: "Pet",
				type: "bigint",
				nullable: true);

			migrationBuilder.CreateIndex(
				name: "IX_Pet_AreaId",
				table: "Pet",
				column: "AreaId");

			migrationBuilder.AddForeignKey(
				name: "FK_Pet_Area_AreaId",
				table: "Pet",
				column: "AreaId",
				principalTable: "Area",
				principalColumn: "Id");
		}
	}
}
