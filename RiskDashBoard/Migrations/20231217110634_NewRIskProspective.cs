using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RiskDashBoard.Migrations
{
    /// <inheritdoc />
    public partial class NewRIskProspective : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RiskType",
                table: "Risks",
                newName: "RiskLevel");

            migrationBuilder.AddColumn<int>(
                name: "RiskTypeDecission",
                table: "Phase",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RiskTypeDecission",
                table: "Phase");

            migrationBuilder.RenameColumn(
                name: "RiskLevel",
                table: "Risks",
                newName: "RiskType");
        }
    }
}
