using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetCoffee.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updatePetCoffeeShopTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubmittingEvent_Account_SenderId",
                table: "SubmittingEvent");

            migrationBuilder.DropIndex(
                name: "IX_SubmittingEvent_SenderId",
                table: "SubmittingEvent");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "SubmittingEvent");

            migrationBuilder.DropColumn(
                name: "SenderId",
                table: "SubmittingEvent");

            migrationBuilder.DropColumn(
                name: "PetCafeShopId",
                table: "Event");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "SubmittingEventField",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "CreatedById",
                table: "SubmittingEventField",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "SubmittingEventField",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "SubmittingEventField",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "SubmittingEvent",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "CreatedById",
                table: "SubmittingEvent",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "SubmittingEvent",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "SubmittingEvent",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "TaxCode",
                table: "PetCoffeeShop",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_SubmittingEventField_CreatedById",
                table: "SubmittingEventField",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_SubmittingEvent_CreatedById",
                table: "SubmittingEvent",
                column: "CreatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_SubmittingEvent_Account_CreatedById",
                table: "SubmittingEvent",
                column: "CreatedById",
                principalTable: "Account",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SubmittingEventField_Account_CreatedById",
                table: "SubmittingEventField",
                column: "CreatedById",
                principalTable: "Account",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubmittingEvent_Account_CreatedById",
                table: "SubmittingEvent");

            migrationBuilder.DropForeignKey(
                name: "FK_SubmittingEventField_Account_CreatedById",
                table: "SubmittingEventField");

            migrationBuilder.DropIndex(
                name: "IX_SubmittingEventField_CreatedById",
                table: "SubmittingEventField");

            migrationBuilder.DropIndex(
                name: "IX_SubmittingEvent_CreatedById",
                table: "SubmittingEvent");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "SubmittingEventField");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "SubmittingEventField");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "SubmittingEventField");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "SubmittingEventField");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "SubmittingEvent");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "SubmittingEvent");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "SubmittingEvent");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "SubmittingEvent");

            migrationBuilder.DropColumn(
                name: "TaxCode",
                table: "PetCoffeeShop");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "SubmittingEvent",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<long>(
                name: "SenderId",
                table: "SubmittingEvent",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "PetCafeShopId",
                table: "Event",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_SubmittingEvent_SenderId",
                table: "SubmittingEvent",
                column: "SenderId");

            migrationBuilder.AddForeignKey(
                name: "FK_SubmittingEvent_Account_SenderId",
                table: "SubmittingEvent",
                column: "SenderId",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
