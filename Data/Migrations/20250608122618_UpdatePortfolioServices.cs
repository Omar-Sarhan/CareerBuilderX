using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareerBuilderX.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePortfolioServices : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Services_Portfolios_PortfolioId",
                table: "Services");

            migrationBuilder.DropIndex(
                name: "IX_Services_PortfolioId",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "PortfolioId",
                table: "Services");

            migrationBuilder.AddColumn<byte[]>(
                name: "ServiceImg",
                table: "Services",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ServiceImgName",
                table: "Services",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ServiceImgType",
                table: "Services",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "ProjectImg",
                table: "Projects",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "prjectImgType",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "projectImgName",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ServicePortfolios",
                columns: table => new
                {
                    PortfolioId = table.Column<int>(type: "int", nullable: false),
                    ServiceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServicePortfolios", x => new { x.PortfolioId, x.ServiceId });
                    table.ForeignKey(
                        name: "FK_ServicePortfolios_Portfolios_PortfolioId",
                        column: x => x.PortfolioId,
                        principalTable: "Portfolios",
                        principalColumn: "PortfolioId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServicePortfolios_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "ServiceId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ServicePortfolios_ServiceId",
                table: "ServicePortfolios",
                column: "ServiceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ServicePortfolios");

            migrationBuilder.DropColumn(
                name: "ServiceImg",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "ServiceImgName",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "ServiceImgType",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "ProjectImg",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "prjectImgType",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "projectImgName",
                table: "Projects");

            migrationBuilder.AddColumn<int>(
                name: "PortfolioId",
                table: "Services",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Services_PortfolioId",
                table: "Services",
                column: "PortfolioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Services_Portfolios_PortfolioId",
                table: "Services",
                column: "PortfolioId",
                principalTable: "Portfolios",
                principalColumn: "PortfolioId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
