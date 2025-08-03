using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetConnect.DAL.Data.Migrations
{
    /// <inheritdoc />
    public partial class updateUserConnection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserConnection_UserId",
                table: "UserConnection");

            migrationBuilder.CreateIndex(
                name: "IX_UserConnection_UserId",
                table: "UserConnection",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserConnection_UserId",
                table: "UserConnection");

            migrationBuilder.CreateIndex(
                name: "IX_UserConnection_UserId",
                table: "UserConnection",
                column: "UserId",
                unique: true);
        }
    }
}
