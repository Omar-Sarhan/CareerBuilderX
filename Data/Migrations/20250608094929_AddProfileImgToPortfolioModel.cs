using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareerBuilderX.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddProfileImgToPortfolioModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "ProfileImg",
                table: "Portfolios",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "profileImgName",
                table: "Portfolios",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "profileImgType",
                table: "Portfolios",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfileImg",
                table: "Portfolios");

            migrationBuilder.DropColumn(
                name: "profileImgName",
                table: "Portfolios");

            migrationBuilder.DropColumn(
                name: "profileImgType",
                table: "Portfolios");
        }
    }
}
