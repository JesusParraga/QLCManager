using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RiskDashBoard.Migrations
{
    /// <inheritdoc />
    public partial class NewTableAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhaseTypeId",
                table: "Phase");

            migrationBuilder.CreateTable(
                name: "PhaseTypes",
                columns: table => new
                {
                    PhaseTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PhaseTypeName = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhaseTypes", x => x.PhaseTypeId);
                });

            migrationBuilder.CreateTable(
                name: "PhasePhasesType",
                columns: table => new
                {
                    PhaseTypesPhaseTypeId = table.Column<int>(type: "int", nullable: false),
                    PhasesPhaseId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhasePhasesType", x => new { x.PhaseTypesPhaseTypeId, x.PhasesPhaseId });
                    table.ForeignKey(
                        name: "FK_PhasePhasesType_PhaseTypes_PhaseTypesPhaseTypeId",
                        column: x => x.PhaseTypesPhaseTypeId,
                        principalTable: "PhaseTypes",
                        principalColumn: "PhaseTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PhasePhasesType_Phase_PhasesPhaseId",
                        column: x => x.PhasesPhaseId,
                        principalTable: "Phase",
                        principalColumn: "PhaseId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PhaseTypeRisk",
                columns: table => new
                {
                    PhasesTypePhaseTypeId = table.Column<int>(type: "int", nullable: false),
                    RisksRiskId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhaseTypeRisk", x => new { x.PhasesTypePhaseTypeId, x.RisksRiskId });
                    table.ForeignKey(
                        name: "FK_PhaseTypeRisk_PhaseTypes_PhasesTypePhaseTypeId",
                        column: x => x.PhasesTypePhaseTypeId,
                        principalTable: "PhaseTypes",
                        principalColumn: "PhaseTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PhaseTypeRisk_Risks_RisksRiskId",
                        column: x => x.RisksRiskId,
                        principalTable: "Risks",
                        principalColumn: "RiskId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PhasePhasesType_PhasesPhaseId",
                table: "PhasePhasesType",
                column: "PhasesPhaseId");

            migrationBuilder.CreateIndex(
                name: "IX_PhaseTypeRisk_RisksRiskId",
                table: "PhaseTypeRisk",
                column: "RisksRiskId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PhasePhasesType");

            migrationBuilder.DropTable(
                name: "PhaseTypeRisk");

            migrationBuilder.DropTable(
                name: "PhaseTypes");

            migrationBuilder.AddColumn<int>(
                name: "PhaseTypeId",
                table: "Phase",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
