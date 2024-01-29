using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RiskDashBoard.Migrations
{
    /// <inheritdoc />
    public partial class IterationNumberAddedComment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IterationPhaseNumber",
                table: "Comments",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IterationPhaseNumber",
                table: "Comments");
        }
    }
}
