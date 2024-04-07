using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HelloJob.Data.Migrations
{
    public partial class changeRequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Requests_VacancyId",
                table: "Requests");

            migrationBuilder.AddUniqueConstraint(
                name: "UQ_VacancyId_AppUserId",
                table: "Requests",
                columns: new[] { "VacancyId", "AppUserId" });
        }
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "UQ_VacancyId_AppUserId",
                table: "Requests");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_VacancyId",
                table: "Requests",
                column: "VacancyId");
        }
    }
}
