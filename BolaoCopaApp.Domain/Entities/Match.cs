using BolaoCopaApp.Domain.Enums;
using BolaoCopaApp.Domain.ValueObjects;

namespace BolaoCopaApp.Domain.Entities;

public class Match
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string ExternalId { get; set; } = string.Empty;
    public string HomeTeam { get; set; } = string.Empty;
    public string AwayTeam { get; set; } = string.Empty;
    public string? Group { get; set; }
    public MatchRound Round { get; set; }
    public DateTime MatchDate { get; set; }
    public MatchStatus Status { get; set; } = MatchStatus.Open;
    public Score? HomeScore { get; set; }
    public Score? AwayScore { get; set; }
    public string? Resolution { get; set; } // "Normal" | "ExtraTime" | "Penalties"
    public string? WinnerTeam { get; set; } // required when score is tied (ExtraTime/Penalties)
}
