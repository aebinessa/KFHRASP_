using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KFHRBackEnd.Migrations
{
    /// <inheritdoc />
    public partial class AddPoitns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RewardPoints",
                table: "RecommendedCertificates",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RewardPoints",
                table: "RecommendedCertificates");

        }
    }
}
