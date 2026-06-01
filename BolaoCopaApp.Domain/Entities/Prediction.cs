using BolaoCopaApp.Domain.ValueObjects;

namespace BolaoCopaApp.Domain.Entities;

public class Prediction
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public Guid MatchId { get; set; }
    public Score HomeScore { get; set; } = default!;
    public Score AwayScore { get; set; } = default!;
    public int Points { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    public User User { get; set; } = default!;
    public Match Match { get; set; } = default!;
}
