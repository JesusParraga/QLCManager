using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RiskDashBoard.Migrations
{
    /// <inheritdoc />
    public partial class historicImprovement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "HistoricPhases",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<int>(
                name: "DecissionId",
                table: "HistoricPhases",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DecissionId",
                table: "HistoricPhases");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "HistoricPhases",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);
        }
    }
}
