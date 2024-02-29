using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetCoffee.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateNameOfReport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Setting_Account_CreatedById",
                table: "Setting");

            migrationBuilder.DropForeignKey(
                name: "FK_Setting_Comment_CommentId",
                table: "Setting");

            migrationBuilder.DropForeignKey(
                name: "FK_Setting_Post_PostID",
                table: "Setting");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Setting",
                table: "Setting");

            migrationBuilder.RenameTable(
                name: "Setting",
                newName: "Report");

            migrationBuilder.RenameIndex(
                name: "IX_Setting_PostID",
                table: "Report",
                newName: "IX_Report_PostID");

            migrationBuilder.RenameIndex(
                name: "IX_Setting_CreatedById",
                table: "Report",
                newName: "IX_Report_CreatedById");

            migrationBuilder.RenameIndex(
                name: "IX_Setting_CommentId",
                table: "Report",
                newName: "IX_Report_CommentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Report",
                table: "Report",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Report_Account_CreatedById",
                table: "Report",
                column: "CreatedById",
                principalTable: "Account",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Report_Comment_CommentId",
                table: "Report",
                column: "CommentId",
                principalTable: "Comment",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Report_Post_PostID",
                table: "Report",
                column: "PostID",
                principalTable: "Post",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Report_Account_CreatedById",
                table: "Report");

            migrationBuilder.DropForeignKey(
                name: "FK_Report_Comment_CommentId",
                table: "Report");

            migrationBuilder.DropForeignKey(
                name: "FK_Report_Post_PostID",
                table: "Report");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Report",
                table: "Report");

            migrationBuilder.RenameTable(
                name: "Report",
                newName: "Setting");

            migrationBuilder.RenameIndex(
                name: "IX_Report_PostID",
                table: "Setting",
                newName: "IX_Setting_PostID");

            migrationBuilder.RenameIndex(
                name: "IX_Report_CreatedById",
                table: "Setting",
                newName: "IX_Setting_CreatedById");

            migrationBuilder.RenameIndex(
                name: "IX_Report_CommentId",
                table: "Setting",
                newName: "IX_Setting_CommentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Setting",
                table: "Setting",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Setting_Account_CreatedById",
                table: "Setting",
                column: "CreatedById",
                principalTable: "Account",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Setting_Comment_CommentId",
                table: "Setting",
                column: "CommentId",
                principalTable: "Comment",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Setting_Post_PostID",
                table: "Setting",
                column: "PostID",
                principalTable: "Post",
                principalColumn: "Id");
        }
    }
}
