using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetConnect.DAL.Data.Migrations
{
    /// <inheritdoc />
    public partial class BlogStructureModified : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Media",
                table: "Blogs",
                type: "varchar(200)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(200)");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Blogs",
                type: "varchar(200)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "excerpt",
                table: "Blogs",
                type: "varchar(500)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "excerpt",
                table: "Blogs");

            migrationBuilder.AlterColumn<string>(
                name: "Media",
                table: "Blogs",
                type: "varchar(200)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(200)",
                oldNullable: true);
        }
    }
}
