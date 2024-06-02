using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KFHRBackEnd.Migrations
{
    /// <inheritdoc />
    public partial class PositionName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Positions_PositionIdID",
                table: "Employees");

            migrationBuilder.DropTable(
                name: "Positions");

            migrationBuilder.DropIndex(
                name: "IX_Employees_PositionIdID",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "PositionIdID",
                table: "Employees");

            migrationBuilder.AddColumn<string>(
                name: "PositionName",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PositionName",
                table: "Employees");

            migrationBuilder.AddColumn<int>(
                name: "PositionIdID",
                table: "Employees",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Positions",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TitleString = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Positions", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employees_PositionIdID",
                table: "Employees",
                column: "PositionIdID");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Positions_PositionIdID",
                table: "Employees",
                column: "PositionIdID",
                principalTable: "Positions",
                principalColumn: "ID");
        }
    }
}
