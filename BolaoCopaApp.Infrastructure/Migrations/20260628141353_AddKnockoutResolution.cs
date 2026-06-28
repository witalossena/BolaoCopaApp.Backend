using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BolaoCopaApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddKnockoutResolution : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.AddColumn<string>(
                name: "Resolution",
                table: "KnockoutPredictions",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExtraTimeAwayScore",
                table: "KnockoutPredictions");

            migrationBuilder.DropColumn(
                name: "ExtraTimeHomeScore",
                table: "KnockoutPredictions");

            migrationBuilder.DropColumn(
                name: "Resolution",
                table: "KnockoutPredictions");
        }
    }
}
