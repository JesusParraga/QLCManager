using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RiskDashBoard.Migrations
{
    /// <inheritdoc />
    public partial class FKRenamingProjectMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectPhaseRisk_Phase_PhasesPhaseId",
                table: "ProjectPhaseRisk");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectPhaseRisk_Risks_RisksRiskId",
                table: "ProjectPhaseRisk");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectPhaseRisk",
                table: "ProjectPhaseRisk");

            migrationBuilder.RenameTable(
                name: "ProjectPhaseRisk",
                newName: "PhaseRisk");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectPhaseRisk_RisksRiskId",
                table: "PhaseRisk",
                newName: "IX_PhaseRisk_RisksRiskId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PhaseRisk",
                table: "PhaseRisk",
                columns: new[] { "PhasesPhaseId", "RisksRiskId" });

            migrationBuilder.AddForeignKey(
                name: "FK_PhaseRisk_Phase_PhasesPhaseId",
                table: "PhaseRisk",
                column: "PhasesPhaseId",
                principalTable: "Phase",
                principalColumn: "PhaseId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PhaseRisk_Risks_RisksRiskId",
                table: "PhaseRisk",
                column: "RisksRiskId",
                principalTable: "Risks",
                principalColumn: "RiskId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PhaseRisk_Phase_PhasesPhaseId",
                table: "PhaseRisk");

            migrationBuilder.DropForeignKey(
                name: "FK_PhaseRisk_Risks_RisksRiskId",
                table: "PhaseRisk");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PhaseRisk",
                table: "PhaseRisk");

            migrationBuilder.RenameTable(
                name: "PhaseRisk",
                newName: "ProjectPhaseRisk");

            migrationBuilder.RenameIndex(
                name: "IX_PhaseRisk_RisksRiskId",
                table: "ProjectPhaseRisk",
                newName: "IX_ProjectPhaseRisk_RisksRiskId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectPhaseRisk",
                table: "ProjectPhaseRisk",
                columns: new[] { "PhasesPhaseId", "RisksRiskId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectPhaseRisk_Phase_PhasesPhaseId",
                table: "ProjectPhaseRisk",
                column: "PhasesPhaseId",
                principalTable: "Phase",
                principalColumn: "PhaseId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectPhaseRisk_Risks_RisksRiskId",
                table: "ProjectPhaseRisk",
                column: "RisksRiskId",
                principalTable: "Risks",
                principalColumn: "RiskId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
