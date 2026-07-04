using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BolaoCopaApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRoundOf16Teams : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var updates = new[]
            {
                new { ExternalId = "ko_r16_0", Home = "Canadá",        Away = "Marrocos",   Date = "2026-07-04 17:00:00" },
                new { ExternalId = "ko_r16_1", Home = "Paraguai",      Away = "França",     Date = "2026-07-04 21:00:00" },
                new { ExternalId = "ko_r16_2", Home = "Brasil",        Away = "Noruega",    Date = "2026-07-05 20:00:00" },
                new { ExternalId = "ko_r16_3", Home = "México",        Away = "Inglaterra", Date = "2026-07-06 00:00:00" },
                new { ExternalId = "ko_r16_4", Home = "Portugal",      Away = "Espanha",    Date = "2026-07-06 19:00:00" },
                new { ExternalId = "ko_r16_5", Home = "Estados Unidos",Away = "Bélgica",    Date = "2026-07-07 00:00:00" },
                new { ExternalId = "ko_r16_6", Home = "Argentina",     Away = "Egito",      Date = "2026-07-07 16:00:00" },
                new { ExternalId = "ko_r16_7", Home = "Suíça",         Away = "Colômbia",   Date = "2026-07-07 20:00:00" },
            };

            foreach (var u in updates)
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var reverts = new[]
            {
                new { ExternalId = "ko_r16_0", Home = "Vencedor R32-1",  Away = "Vencedor R32-2",  Date = "2026-07-05 16:00:00" },
                new { ExternalId = "ko_r16_1", Home = "Vencedor R32-3",  Away = "Vencedor R32-4",  Date = "2026-07-05 19:00:00" },
                new { ExternalId = "ko_r16_2", Home = "Vencedor R32-5",  Away = "Vencedor R32-6",  Date = "2026-07-05 22:00:00" },
                new { ExternalId = "ko_r16_3", Home = "Vencedor R32-7",  Away = "Vencedor R32-8",  Date = "2026-07-06 16:00:00" },
                new { ExternalId = "ko_r16_4", Home = "Vencedor R32-9",  Away = "Vencedor R32-10", Date = "2026-07-06 19:00:00" },
                new { ExternalId = "ko_r16_5", Home = "Vencedor R32-11", Away = "Vencedor R32-12", Date = "2026-07-06 22:00:00" },
                new { ExternalId = "ko_r16_6", Home = "Vencedor R32-13", Away = "Vencedor R32-14", Date = "2026-07-07 16:00:00" },
                new { ExternalId = "ko_r16_7", Home = "Vencedor R32-15", Away = "Vencedor R32-16", Date = "2026-07-07 19:00:00" },
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
