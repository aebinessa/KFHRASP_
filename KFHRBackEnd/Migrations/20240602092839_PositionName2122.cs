using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KFHRBackEnd.Migrations
{
    /// <inheritdoc />
    public partial class PositionName2122 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RewordPoints",
                table: "RecommendedCertificates",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RewordPoints",
                table: "RecommendedCertificates");
        }
    }
}
