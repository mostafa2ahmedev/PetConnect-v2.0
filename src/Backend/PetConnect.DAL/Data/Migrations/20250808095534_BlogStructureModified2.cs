using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetConnect.DAL.Data.Migrations
{
    /// <inheritdoc />
    public partial class BlogStructureModified2 : Migration
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
