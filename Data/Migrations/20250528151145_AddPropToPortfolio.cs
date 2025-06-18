using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareerBuilderX.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPropToPortfolio : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProjectsInfo",
                table: "Portfolios",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ServicesInfo",
                table: "Portfolios",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProjectsInfo",
                table: "Portfolios");

            migrationBuilder.DropColumn(
                name: "ServicesInfo",
                table: "Portfolios");
        }
    }
}
