using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RiskDashBoard.Migrations
{
    /// <inheritdoc />
    public partial class HistoricChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HistoricPhasePhase",
                table: "HistoricPhases");

            migrationBuilder.DropForeignKey(
                name: "FK_Risks_HistoricPhases_HistoricPhaseId",
                table: "Risks");

            migrationBuilder.DropIndex(
                name: "IX_Risks_HistoricPhaseId",
                table: "Risks");

            migrationBuilder.DropIndex(
                name: "IX_HistoricPhases_PhaseId",
                table: "HistoricPhases");

            migrationBuilder.DropColumn(
                name: "HistoricPhaseId",
                table: "Risks");

            migrationBuilder.RenameColumn(
                name: "PhaseId",
                table: "HistoricPhases",
                newName: "UserId");

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "HistoricPhases",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsBack",
                table: "HistoricPhases",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ProjectId",
                table: "HistoricPhases",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "HistoricPhases",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_HistoricPhases_ProjectId",
                table: "HistoricPhases",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_HistoricProject",
                table: "HistoricPhases",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "ProjectId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HistoricProject",
                table: "HistoricPhases");

            migrationBuilder.DropIndex(
                name: "IX_HistoricPhases_ProjectId",
                table: "HistoricPhases");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "HistoricPhases");

            migrationBuilder.DropColumn(
                name: "IsBack",
                table: "HistoricPhases");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "HistoricPhases");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "HistoricPhases");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "HistoricPhases",
                newName: "PhaseId");

            migrationBuilder.AddColumn<int>(
                name: "HistoricPhaseId",
                table: "Risks",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Risks_HistoricPhaseId",
                table: "Risks",
                column: "HistoricPhaseId");

            migrationBuilder.CreateIndex(
                name: "IX_HistoricPhases_PhaseId",
                table: "HistoricPhases",
                column: "PhaseId");

            migrationBuilder.AddForeignKey(
                name: "FK_HistoricPhasePhase",
                table: "HistoricPhases",
                column: "PhaseId",
                principalTable: "Phase",
                principalColumn: "PhaseId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Risks_HistoricPhases_HistoricPhaseId",
                table: "Risks",
                column: "HistoricPhaseId",
                principalTable: "HistoricPhases",
                principalColumn: "HistoricPhaseId");
        }
    }
}
