using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RiskDashBoard.Migrations
{
    /// <inheritdoc />
    public partial class NewRiskInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RiskLevel",
                table: "Risks",
                newName: "RiskProbability");

            migrationBuilder.AddColumn<int>(
                name: "RiskImpact",
                table: "Risks",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RiskImpact",
                table: "Risks");

            migrationBuilder.RenameColumn(
                name: "RiskProbability",
                table: "Risks",
                newName: "RiskLevel");
        }
    }
}
