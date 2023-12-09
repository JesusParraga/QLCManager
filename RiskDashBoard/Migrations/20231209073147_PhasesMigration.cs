using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RiskDashBoard.Migrations
{
    /// <inheritdoc />
    public partial class PhasesMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhaseName",
                table: "Phase");

            migrationBuilder.AddColumn<int>(
                name: "HistoricPhaseId",
                table: "Risks",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCurrentPhase",
                table: "Phase",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "PhaseTypeId",
                table: "Phase",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "HistoricPhases",
                columns: table => new
                {
                    HistoricPhaseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PreviousPhaseId = table.Column<int>(type: "int", nullable: false),
                    CurrentPhaseId = table.Column<int>(type: "int", nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhaseId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistoricPhases", x => x.HistoricPhaseId);
                    table.ForeignKey(
                        name: "FK_HistoricPhasePhase",
                        column: x => x.PhaseId,
                        principalTable: "Phase",
                        principalColumn: "PhaseId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Risks_HistoricPhaseId",
                table: "Risks",
                column: "HistoricPhaseId");

            migrationBuilder.CreateIndex(
                name: "IX_HistoricPhases_PhaseId",
                table: "HistoricPhases",
                column: "PhaseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Risks_HistoricPhases_HistoricPhaseId",
                table: "Risks",
                column: "HistoricPhaseId",
                principalTable: "HistoricPhases",
                principalColumn: "HistoricPhaseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Risks_HistoricPhases_HistoricPhaseId",
                table: "Risks");

            migrationBuilder.DropTable(
                name: "HistoricPhases");

            migrationBuilder.DropIndex(
                name: "IX_Risks_HistoricPhaseId",
                table: "Risks");

            migrationBuilder.DropColumn(
                name: "HistoricPhaseId",
                table: "Risks");

            migrationBuilder.DropColumn(
                name: "IsCurrentPhase",
                table: "Phase");

            migrationBuilder.DropColumn(
                name: "PhaseTypeId",
                table: "Phase");

            migrationBuilder.AddColumn<string>(
                name: "PhaseName",
                table: "Phase",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
