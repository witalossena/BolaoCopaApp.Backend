using BolaoCopaApp.Domain.Entities;
using BolaoCopaApp.Domain.Enums;
using BolaoCopaApp.Infrastructure.Persistence;

namespace BolaoCopaApp.Infrastructure.Persistence.Seeders;

public static class MatchSeeder
{
    public static async Task SeedAsync(BolaoDbContext context)
    {
        if (context.Matches.Any()) return;

        var matches = new List<Match>
        {
            new Match { ExternalId = "m0", HomeTeam = "Mexico", AwayTeam = "Team A2", Group = "A", Round = MatchRound.Group, MatchDate = new DateTime(2026, 6, 11, 19, 0, 0, DateTimeKind.Utc) },
            // Simplified: Not going to type all 96 matches by hand for the mock
            new Match { ExternalId = "m1", HomeTeam = "Canada", AwayTeam = "Team B2", Group = "B", Round = MatchRound.Group, MatchDate = new DateTime(2026, 6, 12, 19, 0, 0, DateTimeKind.Utc) },
            new Match { ExternalId = "m2", HomeTeam = "USA", AwayTeam = "Team D2", Group = "D", Round = MatchRound.Group, MatchDate = new DateTime(2026, 6, 12, 22, 0, 0, DateTimeKind.Utc) }
        };

        await context.Matches.AddRangeAsync(matches);
        
        if (!context.Tournaments.Any())
        {
            await context.Tournaments.AddAsync(new Tournament { Season = 2026, IsActive = true, CurrentPhase = TournamentPhase.PreTournament });
        }

        await context.SaveChangesAsync();
    }
}
