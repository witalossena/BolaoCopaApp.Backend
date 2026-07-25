namespace BolaoCopaApp.Application.DTOs;

public record RegisterRequest(string Name, string Handle, string Email, string Password);
public record LoginRequest(string Email, string Password);
public record AuthResponse(string Token, UserDto User);

public record PointsDto(int GroupPts, int KnockoutPts, int SpecialPts, int Total, int ExactCount, double ExactRate);
public record UserDto(Guid Id, string Name, string Handle, bool IsPaid, string Role, PointsDto Points, bool IsPredictionUnlocked);

public record MatchDto(string Id, string ExternalId, string HomeTeam, string AwayTeam, string Group, string Round, DateTime MatchDate, string Status, int? RealHome, int? RealAway);
public record PredictionDto(string MatchId, int HomeScore, int AwayScore, int? Points);
public record GroupRankDto(string Group, string FirstTeam, string SecondTeam, string? ThirdTeam, string? FourthTeam, int? Points);
public record SpecialPredictionDto(string? Champion, string? RunnerUp, string? ThirdPlace, string? OtherFinalist, string? TopScorer, string? MostAssists, string? MVP, string? GoldenBoy, int? TotalPoints);
public record KnockoutPredictionDto(string MatchId, string WinnerTeam, int? HomeScore, int? AwayScore, string? Resolution, int? Points);

public record RankingEntryDto(int Position, string Name, string Handle, int GroupPts, int SpecialPts, int Total, bool IsPaid);
public record PerformanceDto(int TotalPts, int Position, int GapToLeader, PointsDto Breakdown, double ExactRate);

public record AdminUserDto(Guid Id, string Name, string Handle, int TotalPts, bool IsPaid, bool IsPredictionUnlocked);
public record AdminStatsDto(int TotalUsers, int PaidCount, int PendingCount, int TotalMatches);

public record MatchPredictionSummaryDto(string ExternalId, int HomeScore, int AwayScore, int? Points);
public record GroupRankSummaryDto(string Group, string FirstTeam, string SecondTeam, string? ThirdTeam, string? FourthTeam, int Points);
public record KnockoutPredictionSummaryDto(string ExternalId, string WinnerTeam, int? HomeScore, int? AwayScore, string? Resolution, int Points);
public record UserPredictionsDto(IEnumerable<MatchPredictionSummaryDto> MatchPredictions, IEnumerable<GroupRankSummaryDto> GroupRanks, IEnumerable<KnockoutPredictionSummaryDto> KnockoutPredictions, SpecialPredictionDto? Specials);

public record GroupResultDto(string Group, string FirstTeam, string SecondTeam, string? ThirdTeam, string? FourthTeam);
public record GroupResultSummaryDto(string Group, string FirstTeam, string SecondTeam, string? ThirdTeam, string? FourthTeam);
public record UpdateMatchTeamsDto(string HomeTeam, string AwayTeam);
public class MatchResultRequestDto
{
    public string MatchId { get; set; } = string.Empty;
    public int HomeScore { get; set; }
    public int AwayScore { get; set; }
    public string? Resolution { get; set; }
    public string? WinnerTeam { get; set; }
}

public class LiveScoreDto
{
    public int HomeScore { get; set; }
    public int AwayScore { get; set; }
}

public record TournamentInfoDto(string Phase, decimal PrizePool, bool ArePredictionsLocked);

public record ConfirmPaymentRequest(string Handle, decimal Amount);

public record BetsReportUserDto(string Name, string Handle, int MatchPts, int GroupPts, int KnockoutPts, int SpecialPts, int Total);
public record BetsReportMatchPredDto(string UserName, string UserHandle, string HomeTeam, string AwayTeam, string Group, string Round, string Prediction, string RealResult, int Points);
public record BetsReportGroupPredDto(string UserName, string UserHandle, string Group, string First, string Second, string? Third, string? Fourth, string RealFirst, string RealSecond, string? RealThird, string? RealFourth, int Points);
public record BetsReportKnockoutPredDto(string UserName, string UserHandle, string HomeTeam, string AwayTeam, string Round, string WinnerTeam, string? Resolution, string RealWinner, string? RealResolution, int Points);
public record BetsReportSpecialDto(string UserName, string UserHandle, string? Champion, string? RunnerUp, string? ThirdPlace, string? OtherFinalist, string? TopScorer, string? MostAssists, string? MVP, string? GoldenBoy, int Points);
public record BetsReportDto(
    IEnumerable<BetsReportUserDto> Users,
    IEnumerable<BetsReportMatchPredDto> MatchPredictions,
    IEnumerable<BetsReportGroupPredDto> GroupPredictions,
    IEnumerable<BetsReportKnockoutPredDto> KnockoutPredictions,
    IEnumerable<BetsReportSpecialDto> SpecialPredictions);

public record PredictionHistoryItemDto(
    string HomeTeam, string AwayTeam, string Group,
    int PredictedHome, int PredictedAway,
    int RealHome, int RealAway,
    int Points, DateTime MatchDate);
