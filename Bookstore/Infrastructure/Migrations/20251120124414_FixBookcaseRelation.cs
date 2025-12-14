using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookstore.Migrations
{
    /// <inheritdoc />
    public partial class FixBookcaseRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Books",
                table: "Bookcases");

            migrationBuilder.CreateIndex(
                name: "IX_Books_BookcaseId",
                table: "Books",
                column: "BookcaseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Bookcases_BookcaseId",
                table: "Books",
                column: "BookcaseId",
                principalTable: "Bookcases",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Bookcases_BookcaseId",
                table: "Books");

            migrationBuilder.DropIndex(
                name: "IX_Books_BookcaseId",
                table: "Books");

            migrationBuilder.AddColumn<string>(
                name: "Books",
                table: "Bookcases",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
