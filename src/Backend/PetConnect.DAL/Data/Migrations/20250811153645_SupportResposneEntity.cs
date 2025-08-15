using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetConnect.DAL.Data.Migrations
{
    /// <inheritdoc />
    public partial class SupportResposneEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SupportResponses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Message = table.Column<string>(type: "varchar(max)", nullable: false),
                    SupportRequestId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupportResponses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SupportResponses_SupportRequests_SupportRequestId",
                        column: x => x.SupportRequestId,
                        principalTable: "SupportRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SupportResponses_SupportRequestId",
                table: "SupportResponses",
                column: "SupportRequestId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SupportResponses");
        }
    }
}
