using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HelloJob.Data.Migrations
{
    public partial class DeletedExperiencetable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Resumes_Experiences_ExperienceId",
                table: "Resumes");

            migrationBuilder.DropTable(
                name: "Experiences");

            migrationBuilder.DropIndex(
                name: "IX_Resumes_ExperienceId",
                table: "Resumes");

            migrationBuilder.DropColumn(
                name: "ExperienceId",
                table: "Resumes");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ExperienceId",
                table: "Resumes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Experiences",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Experiences", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Resumes_ExperienceId",
                table: "Resumes",
                column: "ExperienceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Resumes_Experiences_ExperienceId",
                table: "Resumes",
                column: "ExperienceId",
                principalTable: "Experiences",
                principalColumn: "Id");
        }
    }
}
