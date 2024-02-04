using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RiskDashBoard.Migrations
{
    /// <inheritdoc />
    public partial class HistoricModify : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "projectStatus",
                table: "Projects",
                newName: "ProjectStatus");

            migrationBuilder.AddColumn<int>(
                name: "PhaseRiskEvaluation",
                table: "HistoricPhases",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhaseRiskEvaluation",
                table: "HistoricPhases");

            migrationBuilder.RenameColumn(
                name: "ProjectStatus",
                table: "Projects",
                newName: "projectStatus");
        }
    }
}
