namespace BolaoCopaApp.Domain.Entities;

public class GroupResult
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Group { get; set; } = string.Empty;
    public string FirstTeam { get; set; } = string.Empty;
    public string SecondTeam { get; set; } = string.Empty;
    public string? ThirdTeam { get; set; }
    public string? FourthTeam { get; set; }
}
