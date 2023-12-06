using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RiskDashBoard.Migrations
{
    /// <inheritdoc />
    public partial class FKPhaseProjectMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Phase_Projects_ProjectId",
                table: "Phase");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectPhaseRisk_Phase_PhasesProjectPhaseId",
                table: "ProjectPhaseRisk");

            migrationBuilder.RenameColumn(
                name: "PhasesProjectPhaseId",
                table: "ProjectPhaseRisk",
                newName: "PhasesPhaseId");

            migrationBuilder.RenameColumn(
                name: "ProjectPhaseName",
                table: "Phase",
                newName: "PhaseName");

            migrationBuilder.RenameColumn(
                name: "ProjectPhaseId",
                table: "Phase",
                newName: "PhaseId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectPhase",
                table: "Phase",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "ProjectId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectPhaseRisk_Phase_PhasesPhaseId",
                table: "ProjectPhaseRisk",
                column: "PhasesPhaseId",
                principalTable: "Phase",
                principalColumn: "PhaseId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectPhase",
                table: "Phase");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectPhaseRisk_Phase_PhasesPhaseId",
                table: "ProjectPhaseRisk");

            migrationBuilder.RenameColumn(
                name: "PhasesPhaseId",
                table: "ProjectPhaseRisk",
                newName: "PhasesProjectPhaseId");

            migrationBuilder.RenameColumn(
                name: "PhaseName",
                table: "Phase",
                newName: "ProjectPhaseName");

            migrationBuilder.RenameColumn(
                name: "PhaseId",
                table: "Phase",
                newName: "ProjectPhaseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Phase_Projects_ProjectId",
                table: "Phase",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "ProjectId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectPhaseRisk_Phase_PhasesProjectPhaseId",
                table: "ProjectPhaseRisk",
                column: "PhasesProjectPhaseId",
                principalTable: "Phase",
                principalColumn: "ProjectPhaseId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
