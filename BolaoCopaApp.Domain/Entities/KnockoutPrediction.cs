namespace BolaoCopaApp.Domain.Entities;

public class KnockoutPrediction
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public Guid MatchId { get; set; }
    public string WinnerTeam { get; set; } = string.Empty;
    public int? HomeScore { get; set; }
    public int? AwayScore { get; set; }
    public int Points { get; set; }

    public User User { get; set; } = default!;
    public Match Match { get; set; } = default!;
}
