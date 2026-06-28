using System.Globalization;
using System.Text;
using BolaoCopaApp.Domain.Entities;
using BolaoCopaApp.Domain.Enums;
using BolaoCopaApp.Domain.ValueObjects;
using BolaoCopaApp.Infrastructure.Persistence;

namespace BolaoCopaApp.Infrastructure.Persistence.Seeders;

public static class CsvDataSeeder
{
    private static readonly string SeedDataPath = Path.Combine(AppContext.BaseDirectory, "SeedData");

    public static async Task SeedAsync(BolaoDbContext context)
    {
        await SeedTournamentAsync(context);
        await SeedMatchesAsync(context);
        await SeedUsersAsync(context);
        await SeedPredictionsAsync(context);
        await SeedGroupRankPredictionsAsync(context);
        await SeedSpecialPredictionsAsync(context);
    }

    private static async Task SeedTournamentAsync(BolaoDbContext context)
    {
        if (context.Tournaments.Any()) return;
        var file = Path.Combine(SeedDataPath, "tournaments.csv");
        if (!File.Exists(file)) return;

        // Id;Season;IsActive;CurrentPhase;PrizePool;ArePredictionsLocked
        foreach (var cols in ReadCsv(file).Skip(1))
        {
            context.Tournaments.Add(new Tournament
            {
                Id = Guid.Parse(cols[0]),
                Season = int.Parse(cols[1]),
                IsActive = bool.Parse(cols[2]),
                CurrentPhase = (TournamentPhase)int.Parse(cols[3]),
                PrizePool = decimal.Parse(cols[4], CultureInfo.InvariantCulture),
                ArePredictionsLocked = bool.Parse(cols[5])
            });
        }
        await context.SaveChangesAsync();
    }

    private static async Task SeedMatchesAsync(BolaoDbContext context)
    {
        if (context.Matches.Any()) return;
        var file = Path.Combine(SeedDataPath, "matches.csv");
        if (!File.Exists(file)) return;

        // Id;ExternalId;HomeTeam;AwayTeam;Group;Round;MatchDate;Status;HomeScore;AwayScore
        var batch = new List<Match>();
        foreach (var cols in ReadCsv(file).Skip(1))
        {
            var homeScore = string.IsNullOrEmpty(cols[8]) ? (int?)null : int.Parse(cols[8]);
            var awayScore = string.IsNullOrEmpty(cols[9]) ? (int?)null : int.Parse(cols[9]);
            batch.Add(new Match
            {
                Id = Guid.Parse(cols[0]),
                ExternalId = cols[1],
                HomeTeam = cols[2],
                AwayTeam = cols[3],
                Group = string.IsNullOrEmpty(cols[4]) ? null : cols[4],
                Round = Enum.Parse<MatchRound>(cols[5]),
                MatchDate = DateTimeOffset.Parse(cols[6], CultureInfo.InvariantCulture).UtcDateTime,
                Status = Enum.Parse<MatchStatus>(cols[7]),
                HomeScore = homeScore.HasValue ? new Score(homeScore.Value) : null,
                AwayScore = awayScore.HasValue ? new Score(awayScore.Value) : null,
            });
        }
        await context.Matches.AddRangeAsync(batch);
        await context.SaveChangesAsync();
    }

    private static async Task SeedUsersAsync(BolaoDbContext context)
    {
        if (context.Users.Any()) return;
        var file = Path.Combine(SeedDataPath, "users.csv");
        if (!File.Exists(file)) return;

        // Id;Name;Handle;Email;PasswordHash;IsPaid;Role;CreatedAt;PaidAmount;IsPredictionUnlocked
        var batch = new List<User>();
        foreach (var cols in ReadCsv(file).Skip(1))
        {
            try
            {
                batch.Add(new User
                {
                    Id = Guid.Parse(cols[0]),
                    Name = cols[1].Trim(),
                    Handle = new Handle(cols[2]),
                    Email = new Email(cols[3]),
                    PasswordHash = cols[4],
                    IsPaid = bool.Parse(cols[5]),
                    Role = Enum.Parse<Role>(cols[6]),
                    CreatedAt = DateTimeOffset.Parse(cols[7], CultureInfo.InvariantCulture).UtcDateTime,
                    PaidAmount = decimal.Parse(cols[8], CultureInfo.InvariantCulture),
                    IsPredictionUnlocked = bool.Parse(cols[9])
                });
            }
            catch { /* skip invalid rows */ }
        }
        await context.Users.AddRangeAsync(batch);
        await context.SaveChangesAsync();
    }

    private static async Task SeedPredictionsAsync(BolaoDbContext context)
    {
        if (context.Predictions.Any()) return;
        var file = Path.Combine(SeedDataPath, "predictions.csv");
        if (!File.Exists(file)) return;

        // Id;UserId;MatchId;Points;CreatedAt;UpdatedAt;HomeScore;AwayScore
        var validUserIds = context.Users.Select(u => u.Id).ToHashSet();
        var validMatchIds = context.Matches.Select(m => m.Id).ToHashSet();

        Console.WriteLine($"[CsvSeeder] Predictions: {validUserIds.Count} users, {validMatchIds.Count} matches loaded.");

        var batch = new List<Prediction>();
        int skippedFk = 0, skippedErr = 0, lineNum = 0;
        foreach (var cols in ReadCsv(file).Skip(1))
        {
            lineNum++;
            try
            {
                var userId = Guid.Parse(cols[1]);
                var matchId = Guid.Parse(cols[2]);
                if (!validUserIds.Contains(userId) || !validMatchIds.Contains(matchId))
                {
                    skippedFk++;
                    if (skippedFk <= 3)
                        Console.WriteLine($"[CsvSeeder] FK miss line {lineNum}: userId={userId} inUsers={validUserIds.Contains(userId)}, matchId={matchId} inMatches={validMatchIds.Contains(matchId)}");
                    continue;
                }

                batch.Add(new Prediction
                {
                    Id = Guid.Parse(cols[0]),
                    UserId = userId,
                    MatchId = matchId,
                    Points = int.Parse(cols[3]),
                    CreatedAt = DateTimeOffset.Parse(cols[4], CultureInfo.InvariantCulture).UtcDateTime,
                    UpdatedAt = string.IsNullOrEmpty(cols[5]) ? null : DateTimeOffset.Parse(cols[5], CultureInfo.InvariantCulture).UtcDateTime,
                    HomeScore = new Score(int.Parse(cols[6])),
                    AwayScore = new Score(int.Parse(cols[7]))
                });
            }
            catch (Exception ex)
            {
                skippedErr++;
                if (skippedErr <= 3)
                    Console.WriteLine($"[CsvSeeder] Error line {lineNum}: {ex.Message} | cols={string.Join("|", cols)}");
            }
        }
        Console.WriteLine($"[CsvSeeder] Predictions: {batch.Count} to insert, {skippedFk} skipped FK, {skippedErr} skipped error.");
        await context.Predictions.AddRangeAsync(batch);
        await context.SaveChangesAsync();
    }

    private static async Task SeedGroupRankPredictionsAsync(BolaoDbContext context)
    {
        if (context.GroupRankPredictions.Any()) return;
        var file = Path.Combine(SeedDataPath, "group_rank_predictions.csv");
        if (!File.Exists(file)) return;

        // Id;UserId;Group;FirstTeam;SecondTeam;Points;FourthTeam;ThirdTeam
        var validUserIds = context.Users.Select(u => u.Id).ToHashSet();

        var batch = new List<GroupRankPrediction>();
        foreach (var cols in ReadCsv(file).Skip(1))
        {
            try
            {
                var userId = Guid.Parse(cols[1]);
                if (!validUserIds.Contains(userId)) continue;

                batch.Add(new GroupRankPrediction
                {
                    Id = Guid.Parse(cols[0]),
                    UserId = userId,
                    Group = cols[2],
                    FirstTeam = cols[3],
                    SecondTeam = cols[4],
                    Points = int.Parse(cols[5]),
                    FourthTeam = string.IsNullOrEmpty(cols[6]) ? null : cols[6],
                    ThirdTeam = string.IsNullOrEmpty(cols[7]) ? null : cols[7]
                });
            }
            catch { /* skip invalid rows */ }
        }
        await context.GroupRankPredictions.AddRangeAsync(batch);
        await context.SaveChangesAsync();
    }

    private static async Task SeedSpecialPredictionsAsync(BolaoDbContext context)
    {
        if (context.SpecialPredictions.Any()) return;
        var file = Path.Combine(SeedDataPath, "special_predictions.csv");
        if (!File.Exists(file)) return;

        // Id;UserId;Champion;RunnerUp;ThirdPlace;OtherFinalist;TopScorer;MostAssists;MVP;GoldenBoy;Points
        var validUserIds = context.Users.Select(u => u.Id).ToHashSet();

        var batch = new List<SpecialPrediction>();
        foreach (var cols in ReadCsv(file).Skip(1))
        {
            try
            {
                var userId = Guid.Parse(cols[1]);
                if (!validUserIds.Contains(userId)) continue;

                batch.Add(new SpecialPrediction
                {
                    Id = Guid.Parse(cols[0]),
                    UserId = userId,
                    Champion = NullIfEmpty(cols[2]),
                    RunnerUp = NullIfEmpty(cols[3]),
                    ThirdPlace = NullIfEmpty(cols[4]),
                    OtherFinalist = NullIfEmpty(cols[5]),
                    TopScorer = NullIfEmpty(cols[6]),
                    MostAssists = NullIfEmpty(cols[7]),
                    MVP = NullIfEmpty(cols[8]),
                    GoldenBoy = NullIfEmpty(cols[9]),
                    Points = int.Parse(cols[10])
                });
            }
            catch { /* skip invalid rows */ }
        }
        await context.SpecialPredictions.AddRangeAsync(batch);
        await context.SaveChangesAsync();
    }

    private static IEnumerable<string[]> ReadCsv(string path)
    {
        return File.ReadLines(path, Encoding.UTF8)
                   .Where(l => !string.IsNullOrWhiteSpace(l))
                   .Select(l => l.Split(';').Select(f => f.Trim().Trim('"')).ToArray());
    }

    private static string? NullIfEmpty(string value) =>
        string.IsNullOrEmpty(value) ? null : value;
}
