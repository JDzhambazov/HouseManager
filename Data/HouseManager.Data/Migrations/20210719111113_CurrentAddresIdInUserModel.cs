using Microsoft.EntityFrameworkCore.Migrations;

namespace HouseManager.Data.Migrations
{
    public partial class CurrentAddresIdInUserModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CurrentAddressId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentAddressId",
                table: "AspNetUsers");
        }
    }
}
