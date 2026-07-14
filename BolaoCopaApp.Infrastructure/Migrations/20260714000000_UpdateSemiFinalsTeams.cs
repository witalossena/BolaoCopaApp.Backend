using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BolaoCopaApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSemiFinalsTeams : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                UPDATE ""Matches""
                SET ""HomeTeam"" = 'França',
                    ""AwayTeam"" = 'Espanha',
                    ""MatchDate"" = TIMESTAMP '2026-07-14 19:00:00',
                    ""Status""   = 'Open'
                WHERE ""ExternalId"" = 'ko_sf_0';
            ");

            migrationBuilder.Sql(@"
                UPDATE ""Matches""
                SET ""HomeTeam"" = 'Inglaterra',
                    ""AwayTeam"" = 'Argentina',
                    ""MatchDate"" = TIMESTAMP '2026-07-15 19:00:00',
                    ""Status""   = 'Open'
                WHERE ""ExternalId"" = 'ko_sf_1';
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                UPDATE ""Matches""
                SET ""HomeTeam"" = 'Vencedor QF-1',
                    ""AwayTeam"" = 'Vencedor QF-2',
                    ""MatchDate"" = TIMESTAMP '2026-07-14 19:00:00'
                WHERE ""ExternalId"" = 'ko_sf_0';
            ");

            migrationBuilder.Sql(@"
                UPDATE ""Matches""
                SET ""HomeTeam"" = 'Vencedor QF-3',
                    ""AwayTeam"" = 'Vencedor QF-4',
                    ""MatchDate"" = TIMESTAMP '2026-07-15 19:00:00'
                WHERE ""ExternalId"" = 'ko_sf_1';
            ");
        }
    }
}
