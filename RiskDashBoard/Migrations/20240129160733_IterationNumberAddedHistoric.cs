﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RiskDashBoard.Migrations
{
    /// <inheritdoc />
    public partial class IterationNumberAddedHistoric : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IterationPhaseNumber",
                table: "Comments");

            migrationBuilder.AddColumn<int>(
                name: "IterationPhaseNumber",
                table: "HistoricPhases",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IterationPhaseNumber",
                table: "HistoricPhases");

            migrationBuilder.AddColumn<int>(
                name: "IterationPhaseNumber",
                table: "Comments",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
