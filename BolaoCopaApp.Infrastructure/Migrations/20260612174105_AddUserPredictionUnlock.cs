using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BolaoCopaApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUserPredictionUnlock : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPredictionUnlocked",
                table: "Users",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPredictionUnlocked",
                table: "Users");
        }
    }
}
