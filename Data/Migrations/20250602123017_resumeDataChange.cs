using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareerBuilderX.Data.Migrations
{
    /// <inheritdoc />
    public partial class resumeDataChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SName",
                table: "Resumes",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "SName",
                table: "Portfolios",
                newName: "Title");

            migrationBuilder.AddColumn<string>(
                name: "Major",
                table: "Educations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Major",
                table: "Educations");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Resumes",
                newName: "SName");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Portfolios",
                newName: "SName");
        }
    }
}
