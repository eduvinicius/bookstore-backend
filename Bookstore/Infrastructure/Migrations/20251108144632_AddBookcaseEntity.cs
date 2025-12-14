using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookstore.Migrations
{
    /// <inheritdoc />
    public partial class AddBookcaseEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bookcases",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookcases", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Books_BookcaseId",
                table: "Books",
                column: "BookcaseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Bookcases_BookcaseId",
                table: "Books",
                column: "BookcaseId",
                principalTable: "Bookcases",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Bookcases_BookcaseId",
                table: "Books");

            migrationBuilder.DropTable(
                name: "Bookcases");

            migrationBuilder.DropIndex(
                name: "IX_Books_BookcaseId",
                table: "Books");
        }
    }
}
