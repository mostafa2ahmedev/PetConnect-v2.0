using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetConnect.DAL.Data.Migrations
{
    /// <inheritdoc />
    public partial class SupportServiceModification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "SupportResponses",
                type: "datetime2",
                nullable: true,
                defaultValueSql: "GetDate()");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastActivity",
                table: "SupportResponses",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PictureUrl",
                table: "SupportResponses",
                type: "varchar(200)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "SupportRequests",
                type: "datetime2",
                nullable: true,
                defaultValueSql: "GetDate()");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastActivity",
                table: "SupportRequests",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PictureUrl",
                table: "SupportRequests",
                type: "varchar(200)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Priority",
                table: "SupportRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "FollowUpSupportRequest",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Message = table.Column<string>(type: "varchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GetDate()"),
                    LastActivity = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PictureUrl = table.Column<string>(type: "varchar(200)", nullable: true),
                    SupportRequestId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FollowUpSupportRequest", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FollowUpSupportRequest_SupportRequests_SupportRequestId",
                        column: x => x.SupportRequestId,
                        principalTable: "SupportRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FollowUpSupportRequest_SupportRequestId",
                table: "FollowUpSupportRequest",
                column: "SupportRequestId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FollowUpSupportRequest");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "SupportResponses");

            migrationBuilder.DropColumn(
                name: "LastActivity",
                table: "SupportResponses");

            migrationBuilder.DropColumn(
                name: "PictureUrl",
                table: "SupportResponses");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "SupportRequests");

            migrationBuilder.DropColumn(
                name: "LastActivity",
                table: "SupportRequests");

            migrationBuilder.DropColumn(
                name: "PictureUrl",
                table: "SupportRequests");

            migrationBuilder.DropColumn(
                name: "Priority",
                table: "SupportRequests");
        }
    }
}
