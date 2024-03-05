using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetCoffee.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddrelationshopforItemAndChangeFieldEventField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Items_ItemId",
                table: "Transaction");

            migrationBuilder.DropIndex(
                name: "IX_Transaction_ItemId",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "ItemId",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "OptionValue",
                table: "SubmittingEventField");

            migrationBuilder.DropColumn(
                name: "Order",
                table: "SubmittingEventField");

            migrationBuilder.DropColumn(
                name: "OptionValue",
                table: "EventFields");

            migrationBuilder.DropColumn(
                name: "Order",
                table: "EventFields");

            migrationBuilder.RenameColumn(
                name: "FieldValue",
                table: "SubmittingEventField",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "FieldName",
                table: "SubmittingEventField",
                newName: "Question");

            migrationBuilder.RenameColumn(
                name: "FieldValue",
                table: "EventFields",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "FieldName",
                table: "EventFields",
                newName: "Question");

            migrationBuilder.CreateTable(
                name: "TransactionItem",
                columns: table => new
                {
                    ItemId = table.Column<long>(type: "bigint", nullable: false),
                    TransactionId = table.Column<long>(type: "bigint", nullable: false),
                    TotalItem = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionItem", x => new { x.TransactionId, x.ItemId });
                    table.ForeignKey(
                        name: "FK_TransactionItem_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TransactionItem_Transaction_TransactionId",
                        column: x => x.TransactionId,
                        principalTable: "Transaction",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "WalletItem",
                columns: table => new
                {
                    ItemId = table.Column<long>(type: "bigint", nullable: false),
                    WalletId = table.Column<long>(type: "bigint", nullable: false),
                    TotalItem = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WalletItem", x => new { x.WalletId, x.ItemId });
                    table.ForeignKey(
                        name: "FK_WalletItem_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WalletItem_Wallet_WalletId",
                        column: x => x.WalletId,
                        principalTable: "Wallet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionItem_ItemId",
                table: "TransactionItem",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_WalletItem_ItemId",
                table: "WalletItem",
                column: "ItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TransactionItem");

            migrationBuilder.DropTable(
                name: "WalletItem");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "SubmittingEventField",
                newName: "FieldValue");

            migrationBuilder.RenameColumn(
                name: "Question",
                table: "SubmittingEventField",
                newName: "FieldName");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "EventFields",
                newName: "FieldValue");

            migrationBuilder.RenameColumn(
                name: "Question",
                table: "EventFields",
                newName: "FieldName");

            migrationBuilder.AddColumn<long>(
                name: "ItemId",
                table: "Transaction",
                type: "bigint",
                nullable: true);

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

            migrationBuilder.AddColumn<string>(
                name: "OptionValue",
                table: "EventFields",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "EventFields",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_ItemId",
                table: "Transaction",
                column: "ItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_Items_ItemId",
                table: "Transaction",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "ItemId");
        }
    }
}
