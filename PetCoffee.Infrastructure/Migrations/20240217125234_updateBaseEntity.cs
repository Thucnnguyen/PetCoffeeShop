using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetCoffee.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateBaseEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Account_Account_CreatedById",
                table: "Account");

            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Account_CreatedById",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Account_CreatedById",
                table: "Comment");

            migrationBuilder.DropForeignKey(
                name: "FK_Diaries_Account_CreatedById",
                table: "Diaries");

            migrationBuilder.DropForeignKey(
                name: "FK_Event_Account_CreatedById",
                table: "Event");

            migrationBuilder.DropForeignKey(
                name: "FK_Items_Account_CreatedById",
                table: "Items");

            migrationBuilder.DropForeignKey(
                name: "FK_Notification_Account_CreatedById",
                table: "Notification");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_Account_CreatedById",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_Pet_Account_CreatedById",
                table: "Pet");

            migrationBuilder.DropForeignKey(
                name: "FK_PetCoffeeShop_Account_CreatedById",
                table: "PetCoffeeShop");

            migrationBuilder.DropForeignKey(
                name: "FK_Post_Account_CreatedById",
                table: "Post");

            migrationBuilder.DropForeignKey(
                name: "FK_PostCategories_Account_CreatedById",
                table: "PostCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_PostPetCoffeeShops_Account_CreatedById",
                table: "PostPetCoffeeShops");

            migrationBuilder.DropForeignKey(
                name: "FK_Setting_Account_CreatedById",
                table: "Setting");

            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Account_CreatedById",
                table: "Transaction");

            migrationBuilder.DropForeignKey(
                name: "FK_Wallet_Account_CreatedById",
                table: "Wallet");

            migrationBuilder.AlterColumn<long>(
                name: "CreatedById",
                table: "Wallet",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "CreatedById",
                table: "Transaction",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "CreatedById",
                table: "Setting",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "CreatedById",
                table: "PostPetCoffeeShops",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "CreatedById",
                table: "PostCategories",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "CreatedById",
                table: "Post",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "CreatedById",
                table: "PetCoffeeShop",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "CreatedById",
                table: "Pet",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "CreatedById",
                table: "Order",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "CreatedById",
                table: "Notification",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "CreatedById",
                table: "Items",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "CreatedById",
                table: "Event",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "CreatedById",
                table: "Diaries",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "CreatedById",
                table: "Comment",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "CreatedById",
                table: "Categories",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "CreatedById",
                table: "Account",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_Account_Account_CreatedById",
                table: "Account",
                column: "CreatedById",
                principalTable: "Account",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Account_CreatedById",
                table: "Categories",
                column: "CreatedById",
                principalTable: "Account",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Account_CreatedById",
                table: "Comment",
                column: "CreatedById",
                principalTable: "Account",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Diaries_Account_CreatedById",
                table: "Diaries",
                column: "CreatedById",
                principalTable: "Account",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Event_Account_CreatedById",
                table: "Event",
                column: "CreatedById",
                principalTable: "Account",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Account_CreatedById",
                table: "Items",
                column: "CreatedById",
                principalTable: "Account",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_Account_CreatedById",
                table: "Notification",
                column: "CreatedById",
                principalTable: "Account",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Account_CreatedById",
                table: "Order",
                column: "CreatedById",
                principalTable: "Account",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Pet_Account_CreatedById",
                table: "Pet",
                column: "CreatedById",
                principalTable: "Account",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PetCoffeeShop_Account_CreatedById",
                table: "PetCoffeeShop",
                column: "CreatedById",
                principalTable: "Account",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Post_Account_CreatedById",
                table: "Post",
                column: "CreatedById",
                principalTable: "Account",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PostCategories_Account_CreatedById",
                table: "PostCategories",
                column: "CreatedById",
                principalTable: "Account",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PostPetCoffeeShops_Account_CreatedById",
                table: "PostPetCoffeeShops",
                column: "CreatedById",
                principalTable: "Account",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Setting_Account_CreatedById",
                table: "Setting",
                column: "CreatedById",
                principalTable: "Account",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_Account_CreatedById",
                table: "Transaction",
                column: "CreatedById",
                principalTable: "Account",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Wallet_Account_CreatedById",
                table: "Wallet",
                column: "CreatedById",
                principalTable: "Account",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Account_Account_CreatedById",
                table: "Account");

            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Account_CreatedById",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Account_CreatedById",
                table: "Comment");

            migrationBuilder.DropForeignKey(
                name: "FK_Diaries_Account_CreatedById",
                table: "Diaries");

            migrationBuilder.DropForeignKey(
                name: "FK_Event_Account_CreatedById",
                table: "Event");

            migrationBuilder.DropForeignKey(
                name: "FK_Items_Account_CreatedById",
                table: "Items");

            migrationBuilder.DropForeignKey(
                name: "FK_Notification_Account_CreatedById",
                table: "Notification");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_Account_CreatedById",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_Pet_Account_CreatedById",
                table: "Pet");

            migrationBuilder.DropForeignKey(
                name: "FK_PetCoffeeShop_Account_CreatedById",
                table: "PetCoffeeShop");

            migrationBuilder.DropForeignKey(
                name: "FK_Post_Account_CreatedById",
                table: "Post");

            migrationBuilder.DropForeignKey(
                name: "FK_PostCategories_Account_CreatedById",
                table: "PostCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_PostPetCoffeeShops_Account_CreatedById",
                table: "PostPetCoffeeShops");

            migrationBuilder.DropForeignKey(
                name: "FK_Setting_Account_CreatedById",
                table: "Setting");

            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Account_CreatedById",
                table: "Transaction");

            migrationBuilder.DropForeignKey(
                name: "FK_Wallet_Account_CreatedById",
                table: "Wallet");

            migrationBuilder.AlterColumn<long>(
                name: "CreatedById",
                table: "Wallet",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "CreatedById",
                table: "Transaction",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "CreatedById",
                table: "Setting",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "CreatedById",
                table: "PostPetCoffeeShops",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "CreatedById",
                table: "PostCategories",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "CreatedById",
                table: "Post",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "CreatedById",
                table: "PetCoffeeShop",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "CreatedById",
                table: "Pet",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "CreatedById",
                table: "Order",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "CreatedById",
                table: "Notification",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "CreatedById",
                table: "Items",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "CreatedById",
                table: "Event",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "CreatedById",
                table: "Diaries",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "CreatedById",
                table: "Comment",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "CreatedById",
                table: "Categories",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "CreatedById",
                table: "Account",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Account_Account_CreatedById",
                table: "Account",
                column: "CreatedById",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Account_CreatedById",
                table: "Categories",
                column: "CreatedById",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Account_CreatedById",
                table: "Comment",
                column: "CreatedById",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Diaries_Account_CreatedById",
                table: "Diaries",
                column: "CreatedById",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Event_Account_CreatedById",
                table: "Event",
                column: "CreatedById",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Account_CreatedById",
                table: "Items",
                column: "CreatedById",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_Account_CreatedById",
                table: "Notification",
                column: "CreatedById",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Account_CreatedById",
                table: "Order",
                column: "CreatedById",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Pet_Account_CreatedById",
                table: "Pet",
                column: "CreatedById",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PetCoffeeShop_Account_CreatedById",
                table: "PetCoffeeShop",
                column: "CreatedById",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Post_Account_CreatedById",
                table: "Post",
                column: "CreatedById",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PostCategories_Account_CreatedById",
                table: "PostCategories",
                column: "CreatedById",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PostPetCoffeeShops_Account_CreatedById",
                table: "PostPetCoffeeShops",
                column: "CreatedById",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Setting_Account_CreatedById",
                table: "Setting",
                column: "CreatedById",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_Account_CreatedById",
                table: "Transaction",
                column: "CreatedById",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Wallet_Account_CreatedById",
                table: "Wallet",
                column: "CreatedById",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
