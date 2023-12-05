using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RiskDashBoard.Migrations
{
    /// <inheritdoc />
    public partial class RelashionsRelationMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Phase",
                columns: table => new
                {
                    ProjectPhaseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectPhaseName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProjectId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Phase", x => x.ProjectPhaseId);
                    table.ForeignKey(
                        name: "FK_Phase_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "ProjectId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Risks",
                columns: table => new
                {
                    RiskId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RiskName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RiskDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RiskType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Risks", x => x.RiskId);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    TagId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TagName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.TagId);
                });

            migrationBuilder.CreateTable(
                name: "ProjectPhaseRisk",
                columns: table => new
                {
                    ProjectPhasesProjectPhaseId = table.Column<int>(type: "int", nullable: false),
                    RisksRiskId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectPhaseRisk", x => new { x.ProjectPhasesProjectPhaseId, x.RisksRiskId });
                    table.ForeignKey(
                        name: "FK_ProjectPhaseRisk_Phase_ProjectPhasesProjectPhaseId",
                        column: x => x.ProjectPhasesProjectPhaseId,
                        principalTable: "Phase",
                        principalColumn: "ProjectPhaseId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectPhaseRisk_Risks_RisksRiskId",
                        column: x => x.RisksRiskId,
                        principalTable: "Risks",
                        principalColumn: "RiskId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Phase_ProjectId",
                table: "Phase",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectPhaseRisk_RisksRiskId",
                table: "ProjectPhaseRisk",
                column: "RisksRiskId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectPhaseRisk");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "Phase");

            migrationBuilder.DropTable(
                name: "Risks");
        }
    }
}
