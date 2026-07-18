using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BolaoCopaApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFinalAndThirdPlace : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Final: Espanha vs Argentina, 19/07 15h local NY (19:00 UTC = 16h Brasília)
            migrationBuilder.Sql(@"
                UPDATE ""Matches""
                SET ""HomeTeam"" = 'Espanha',
                    ""AwayTeam"" = 'Argentina',
                    ""MatchDate"" = TIMESTAMP '2026-07-19 19:00:00',
                    ""Status""   = 'Open'
                WHERE ""ExternalId"" = 'ko_final';
            ");

            // 3rd place: França vs Inglaterra, 18/07 17h local Miami (21:00 UTC = 18h Brasília)
            migrationBuilder.Sql(@"
                UPDATE ""Matches""
                SET ""HomeTeam"" = 'França',
                    ""AwayTeam"" = 'Inglaterra',
                    ""MatchDate"" = TIMESTAMP '2026-07-18 21:00:00',
                    ""Status""   = 'Open'
                WHERE ""ExternalId"" = 'ko_3rd';
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                UPDATE ""Matches""
                SET ""HomeTeam"" = 'Vencedor SF-1',
                    ""AwayTeam"" = 'Vencedor SF-2',
                    ""Status""   = 'Open'
                WHERE ""ExternalId"" = 'ko_final';
            ");

            migrationBuilder.Sql(@"
                UPDATE ""Matches""
                SET ""HomeTeam"" = 'Perdedor SF-1',
                    ""AwayTeam"" = 'Perdedor SF-2',
                    ""MatchDate"" = TIMESTAMP '2026-07-18 19:00:00'
                WHERE ""ExternalId"" = 'ko_3rd';
            ");
        }
    }
}
