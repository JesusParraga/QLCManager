using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RiskDashBoard.Migrations
{
    /// <inheritdoc />
    public partial class NewRelationMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectPhaseRisk_Phase_ProjectPhasesProjectPhaseId",
                table: "ProjectPhaseRisk");

            migrationBuilder.RenameColumn(
                name: "ProjectPhasesProjectPhaseId",
                table: "ProjectPhaseRisk",
                newName: "PhasesProjectPhaseId");

            migrationBuilder.AlterColumn<string>(
                name: "TagName",
                table: "Tags",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "Tags",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "RiskTag",
                columns: table => new
                {
                    RisksRiskId = table.Column<int>(type: "int", nullable: false),
                    TagsTagId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiskTag", x => new { x.RisksRiskId, x.TagsTagId });
                    table.ForeignKey(
                        name: "FK_RiskTag_Risks_RisksRiskId",
                        column: x => x.RisksRiskId,
                        principalTable: "Risks",
                        principalColumn: "RiskId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RiskTag_Tags_TagsTagId",
                        column: x => x.TagsTagId,
                        principalTable: "Tags",
                        principalColumn: "TagId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RiskTag_TagsTagId",
                table: "RiskTag",
                column: "TagsTagId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectPhaseRisk_Phase_PhasesProjectPhaseId",
                table: "ProjectPhaseRisk",
                column: "PhasesProjectPhaseId",
                principalTable: "Phase",
                principalColumn: "ProjectPhaseId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectPhaseRisk_Phase_PhasesProjectPhaseId",
                table: "ProjectPhaseRisk");

            migrationBuilder.DropTable(
                name: "RiskTag");

            migrationBuilder.DropColumn(
                name: "Color",
                table: "Tags");

            migrationBuilder.RenameColumn(
                name: "PhasesProjectPhaseId",
                table: "ProjectPhaseRisk",
                newName: "ProjectPhasesProjectPhaseId");

            migrationBuilder.AlterColumn<string>(
                name: "TagName",
                table: "Tags",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectPhaseRisk_Phase_ProjectPhasesProjectPhaseId",
                table: "ProjectPhaseRisk",
                column: "ProjectPhasesProjectPhaseId",
                principalTable: "Phase",
                principalColumn: "ProjectPhaseId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
