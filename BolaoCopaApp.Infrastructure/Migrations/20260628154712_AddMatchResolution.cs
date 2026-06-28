using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BolaoCopaApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMatchResolution : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExtraTimeAwayScore",
                table: "KnockoutPredictions");

            migrationBuilder.DropColumn(
                name: "ExtraTimeHomeScore",
                table: "KnockoutPredictions");

            migrationBuilder.AddColumn<string>(
                name: "Resolution",
                table: "Matches",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Resolution",
                table: "Matches");

            migrationBuilder.AddColumn<int>(
                name: "ExtraTimeAwayScore",
                table: "KnockoutPredictions",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ExtraTimeHomeScore",
                table: "KnockoutPredictions",
                type: "integer",
                nullable: true);
        }
    }
}
