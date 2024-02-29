using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetCoffee.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class changetableSubmitingEventField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubmittingEventField_EventFields_EventFieldId",
                table: "SubmittingEventField");

            migrationBuilder.DropIndex(
                name: "IX_SubmittingEventField_EventFieldId",
                table: "SubmittingEventField");

            migrationBuilder.DropColumn(
                name: "EventFieldId",
                table: "SubmittingEventField");

            migrationBuilder.AddColumn<string>(
                name: "Answer",
                table: "SubmittingEventField",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "FieldName",
                table: "SubmittingEventField",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "FieldValue",
                table: "SubmittingEventField",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<bool>(
                name: "IsOptional",
                table: "SubmittingEventField",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "OptionValue",
                table: "SubmittingEventField",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "SubmittingEventField",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Answer",
                table: "SubmittingEventField");

            migrationBuilder.DropColumn(
                name: "FieldName",
                table: "SubmittingEventField");

            migrationBuilder.DropColumn(
                name: "FieldValue",
                table: "SubmittingEventField");

            migrationBuilder.DropColumn(
                name: "IsOptional",
                table: "SubmittingEventField");

            migrationBuilder.DropColumn(
                name: "OptionValue",
                table: "SubmittingEventField");

            migrationBuilder.DropColumn(
                name: "Order",
                table: "SubmittingEventField");

            migrationBuilder.AddColumn<long>(
                name: "EventFieldId",
                table: "SubmittingEventField",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_SubmittingEventField_EventFieldId",
                table: "SubmittingEventField",
                column: "EventFieldId");

            migrationBuilder.AddForeignKey(
                name: "FK_SubmittingEventField_EventFields_EventFieldId",
                table: "SubmittingEventField",
                column: "EventFieldId",
                principalTable: "EventFields",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
