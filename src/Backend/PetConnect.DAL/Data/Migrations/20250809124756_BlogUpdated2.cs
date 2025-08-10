using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetConnect.DAL.Data.Migrations
{
    /// <inheritdoc />
    public partial class BlogUpdated2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlogCategory");

            migrationBuilder.AddColumn<int>(
                name: "PetCategoryId",
                table: "Blogs",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Blogs_PetCategoryId",
                table: "Blogs",
                column: "PetCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Blogs_PetCategory_PetCategoryId",
                table: "Blogs",
                column: "PetCategoryId",
                principalTable: "PetCategory",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Blogs_PetCategory_PetCategoryId",
                table: "Blogs");

            migrationBuilder.DropIndex(
                name: "IX_Blogs_PetCategoryId",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "PetCategoryId",
                table: "Blogs");

            migrationBuilder.CreateTable(
                name: "BlogCategory",
                columns: table => new
                {
                    BlogId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PetCategoryId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogCategory", x => new { x.BlogId, x.PetCategoryId });
                    table.ForeignKey(
                        name: "FK_BlogCategory_Blogs_BlogId",
                        column: x => x.BlogId,
                        principalTable: "Blogs",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_BlogCategory_PetCategory_PetCategoryId",
                        column: x => x.PetCategoryId,
                        principalTable: "PetCategory",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_BlogCategory_PetCategoryId",
                table: "BlogCategory",
                column: "PetCategoryId");
        }
    }
}
