using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RiskDashBoard.Migrations
{
    /// <inheritdoc />
    public partial class ChangePhaseRiskRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PhaseRisk");

            migrationBuilder.AddColumn<int>(
                name: "PhaseId",
                table: "Risks",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Risks_PhaseId",
                table: "Risks",
                column: "PhaseId");

            migrationBuilder.AddForeignKey(
                name: "FK_RiskPhase",
                table: "Risks",
                column: "PhaseId",
                principalTable: "Phase",
                principalColumn: "PhaseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RiskPhase",
                table: "Risks");

            migrationBuilder.DropIndex(
                name: "IX_Risks_PhaseId",
                table: "Risks");

            migrationBuilder.DropColumn(
                name: "PhaseId",
                table: "Risks");

            migrationBuilder.CreateTable(
                name: "PhaseRisk",
                columns: table => new
                {
                    PhasesPhaseId = table.Column<int>(type: "int", nullable: false),
                    RisksRiskId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhaseRisk", x => new { x.PhasesPhaseId, x.RisksRiskId });
                    table.ForeignKey(
                        name: "FK_PhaseRisk_Phase_PhasesPhaseId",
                        column: x => x.PhasesPhaseId,
                        principalTable: "Phase",
                        principalColumn: "PhaseId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PhaseRisk_Risks_RisksRiskId",
                        column: x => x.RisksRiskId,
                        principalTable: "Risks",
                        principalColumn: "RiskId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PhaseRisk_RisksRiskId",
                table: "PhaseRisk",
                column: "RisksRiskId");
        }
    }
}
