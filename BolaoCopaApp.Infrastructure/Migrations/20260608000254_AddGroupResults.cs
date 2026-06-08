using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BolaoCopaApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddGroupResults : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GroupResults",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Group = table.Column<string>(type: "text", nullable: false),
                    FirstTeam = table.Column<string>(type: "text", nullable: false),
                    SecondTeam = table.Column<string>(type: "text", nullable: false),
                    ThirdTeam = table.Column<string>(type: "text", nullable: true),
                    FourthTeam = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupResults", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GroupResults");
        }
    }
}
