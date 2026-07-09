using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BolaoCopaApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateQuarterFinalTeams : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var updates = new[]
            {
                new { ExternalId = "ko_qf_0", Home = "França",   Away = "Marrocos",   Date = "2026-07-09 20:00:00" },
                new { ExternalId = "ko_qf_1", Home = "Espanha",  Away = "Bélgica",    Date = "2026-07-10 19:00:00" },
                new { ExternalId = "ko_qf_2", Home = "Noruega",  Away = "Inglaterra", Date = "2026-07-11 21:00:00" },
            };

            foreach (var u in updates)
            {
                migrationBuilder.Sql($@"
                    UPDATE ""Matches""
                    SET ""HomeTeam"" = '{u.Home}',
                        ""AwayTeam"" = '{u.Away}',
                        ""MatchDate"" = TIMESTAMP '{u.Date}',
                        ""Status""   = 'Open'
                    WHERE ""ExternalId"" = '{u.ExternalId}';
                ");
            }
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var reverts = new[]
            {
                new { ExternalId = "ko_qf_0", Home = "Vencedor R16-1", Away = "Vencedor R16-2", Date = "2026-07-10 19:00:00" },
                new { ExternalId = "ko_qf_1", Home = "Vencedor R16-3", Away = "Vencedor R16-4", Date = "2026-07-10 22:00:00" },
                new { ExternalId = "ko_qf_2", Home = "Vencedor R16-5", Away = "Vencedor R16-6", Date = "2026-07-11 19:00:00" },
            };

            foreach (var u in reverts)
            {
                migrationBuilder.Sql($@"
                    UPDATE ""Matches""
                    SET ""HomeTeam"" = '{u.Home}',
                        ""AwayTeam"" = '{u.Away}',
                        ""MatchDate"" = TIMESTAMP '{u.Date}'
                    WHERE ""ExternalId"" = '{u.ExternalId}';
                ");
            }
        }
    }
}
