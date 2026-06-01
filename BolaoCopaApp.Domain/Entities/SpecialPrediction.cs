namespace BolaoCopaApp.Domain.Entities;

public class SpecialPrediction
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public string? Champion { get; set; }
    public string? RunnerUp { get; set; }
    public string? ThirdPlace { get; set; }
    public string? OtherFinalist { get; set; }
    public string? TopScorer { get; set; }
    public string? MostAssists { get; set; }
    public string? MVP { get; set; }
    public string? GoldenBoy { get; set; }
    public int Points { get; set; }

    public User User { get; set; } = default!;
}
