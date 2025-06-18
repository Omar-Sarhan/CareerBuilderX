using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareerBuilderX.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangePortfolio : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PortfolioId",
                table: "Skills",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FaceBookLink",
                table: "Portfolios",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SkillsInfo",
                table: "Portfolios",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TwitterLink",
                table: "Portfolios",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Skills_PortfolioId",
                table: "Skills",
                column: "PortfolioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Skills_Portfolios_PortfolioId",
                table: "Skills",
                column: "PortfolioId",
                principalTable: "Portfolios",
                principalColumn: "PortfolioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Skills_Portfolios_PortfolioId",
                table: "Skills");

            migrationBuilder.DropIndex(
                name: "IX_Skills_PortfolioId",
                table: "Skills");

            migrationBuilder.DropColumn(
                name: "PortfolioId",
                table: "Skills");

            migrationBuilder.DropColumn(
                name: "FaceBookLink",
                table: "Portfolios");

            migrationBuilder.DropColumn(
                name: "SkillsInfo",
                table: "Portfolios");

            migrationBuilder.DropColumn(
                name: "TwitterLink",
                table: "Portfolios");
        }
    }
}
