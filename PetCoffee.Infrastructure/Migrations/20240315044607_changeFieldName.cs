using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetCoffee.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class changeFieldName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalSeat",
                table: "Area");

            migrationBuilder.RenameColumn(
                name: "Submitcontent",
                table: "SubmittingEventField",
                newName: "SubmittingContent");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SubmittingContent",
                table: "SubmittingEventField",
                newName: "Submitcontent");

            migrationBuilder.AddColumn<int>(
                name: "TotalSeat",
                table: "Area",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
