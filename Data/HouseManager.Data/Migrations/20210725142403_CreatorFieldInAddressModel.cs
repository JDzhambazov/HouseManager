using Microsoft.EntityFrameworkCore.Migrations;

namespace HouseManager.Data.Migrations
{
    public partial class CreatorFieldInAddressModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatorId",
                table: "Addresses",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_CreatorId",
                table: "Addresses",
                column: "CreatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_AspNetUsers_CreatorId",
                table: "Addresses",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_AspNetUsers_CreatorId",
                table: "Addresses");

            migrationBuilder.DropIndex(
                name: "IX_Addresses_CreatorId",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "Addresses");
        }
    }
}
