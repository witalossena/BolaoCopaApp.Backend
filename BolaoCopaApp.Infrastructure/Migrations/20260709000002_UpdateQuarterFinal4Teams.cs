using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BolaoCopaApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateQuarterFinal4Teams : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                UPDATE ""Matches""
                SET ""HomeTeam"" = 'Argentina',
                    ""AwayTeam"" = 'Suíça',
                    ""MatchDate"" = TIMESTAMP '2026-07-12 01:00:00',
                    ""Status""   = 'Open'
                WHERE ""ExternalId"" = 'ko_qf_3';
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                UPDATE ""Matches""
                SET ""HomeTeam"" = 'Vencedor R16-7',
                    ""AwayTeam"" = 'Vencedor R16-8',
                    ""MatchDate"" = TIMESTAMP '2026-07-11 22:00:00'
                WHERE ""ExternalId"" = 'ko_qf_3';
            ");
        }
    }
}
