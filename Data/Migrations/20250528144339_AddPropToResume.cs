using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareerBuilderX.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPropToResume : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CareerInfo",
                table: "Resumes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CertificationInfo",
                table: "Resumes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EducationInfo",
                table: "Resumes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LanguageInfo",
                table: "Resumes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SkillsInfo",
                table: "Resumes",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CareerInfo",
                table: "Resumes");

            migrationBuilder.DropColumn(
                name: "CertificationInfo",
                table: "Resumes");

            migrationBuilder.DropColumn(
                name: "EducationInfo",
                table: "Resumes");

            migrationBuilder.DropColumn(
                name: "LanguageInfo",
                table: "Resumes");

            migrationBuilder.DropColumn(
                name: "SkillsInfo",
                table: "Resumes");
        }
    }
}
