using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RiskDashBoard.Migrations
{
    /// <inheritdoc />
    public partial class HistoricChangesTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentPhaseId",
                table: "HistoricPhases");

            migrationBuilder.DropColumn(
                name: "PreviousPhaseId",
                table: "HistoricPhases");

            migrationBuilder.AddColumn<string>(
                name: "CurrentPhaseType",
                table: "HistoricPhases",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PreviousPhaseType",
                table: "HistoricPhases",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentPhaseType",
                table: "HistoricPhases");

            migrationBuilder.DropColumn(
                name: "PreviousPhaseType",
                table: "HistoricPhases");

            migrationBuilder.AddColumn<int>(
                name: "CurrentPhaseId",
                table: "HistoricPhases",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PreviousPhaseId",
                table: "HistoricPhases",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
