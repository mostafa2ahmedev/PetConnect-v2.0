using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetConnect.DAL.Data.Migrations
{
    /// <inheritdoc />
    public partial class Altered_CustomerPetAdoptionsModelANDConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerPetAdoptions_Customers_CustomerId",
                table: "CustomerPetAdoptions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CustomerPetAdoptions",
                table: "CustomerPetAdoptions");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "CustomerPetAdoptions",
                newName: "ReceiverCustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_CustomerPetAdoptions_CustomerId",
                table: "CustomerPetAdoptions",
                newName: "IX_CustomerPetAdoptions_ReceiverCustomerId");

            migrationBuilder.AddColumn<string>(
                name: "RequesterCustomerId",
                table: "CustomerPetAdoptions",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CustomerPetAdoptions",
                table: "CustomerPetAdoptions",
                columns: new[] { "PetId", "RequesterCustomerId", "AdoptionDate" });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerPetAdoptions_RequesterCustomerId",
                table: "CustomerPetAdoptions",
                column: "RequesterCustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerPetAdoptions_Customers_ReceiverCustomerId",
                table: "CustomerPetAdoptions",
                column: "ReceiverCustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerPetAdoptions_Customers_RequesterCustomerId",
                table: "CustomerPetAdoptions",
                column: "RequesterCustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerPetAdoptions_Customers_ReceiverCustomerId",
                table: "CustomerPetAdoptions");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerPetAdoptions_Customers_RequesterCustomerId",
                table: "CustomerPetAdoptions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CustomerPetAdoptions",
                table: "CustomerPetAdoptions");

            migrationBuilder.DropIndex(
                name: "IX_CustomerPetAdoptions_RequesterCustomerId",
                table: "CustomerPetAdoptions");

            migrationBuilder.DropColumn(
                name: "RequesterCustomerId",
                table: "CustomerPetAdoptions");

            migrationBuilder.RenameColumn(
                name: "ReceiverCustomerId",
                table: "CustomerPetAdoptions",
                newName: "CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_CustomerPetAdoptions_ReceiverCustomerId",
                table: "CustomerPetAdoptions",
                newName: "IX_CustomerPetAdoptions_CustomerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CustomerPetAdoptions",
                table: "CustomerPetAdoptions",
                columns: new[] { "PetId", "CustomerId" });

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerPetAdoptions_Customers_CustomerId",
                table: "CustomerPetAdoptions",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
