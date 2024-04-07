using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HelloJob.Data.Migrations
{
    public partial class SomeChangesCourseproperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsFree",
                table: "Courses",
                newName: "IsPremium");

            migrationBuilder.AddColumn<int>(
                name: "Price",
                table: "Courses",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "Courses");

            migrationBuilder.RenameColumn(
                name: "IsPremium",
                table: "Courses",
                newName: "IsFree");
        }
    }
}
