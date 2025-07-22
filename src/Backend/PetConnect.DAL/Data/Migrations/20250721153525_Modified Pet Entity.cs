using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetConnect.DAL.Data.Migrations
{
    /// <inheritdoc />
    public partial class ModifiedPetEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Age",
                table: "Pets",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Age",
                table: "Pets");
        }
    }
}
