using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HelloJob.Data.Migrations
{
    public partial class NewUpdatedAll : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActivate",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActivate",
                table: "AspNetUsers");
        }
    }
}
