using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BolaoCopaApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddThirdTeamToGroupRank : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ThirdTeam",
                table: "GroupRankPredictions",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ThirdTeam",
                table: "GroupRankPredictions");
        }
    }
}
