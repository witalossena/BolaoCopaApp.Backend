namespace BolaoCopaApp.Domain.Entities;

public class GroupRankPrediction
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public string Group { get; set; } = string.Empty;
    public string FirstTeam { get; set; } = string.Empty;
    public string SecondTeam { get; set; } = string.Empty;
    public int Points { get; set; }

    public User User { get; set; } = default!;
}
