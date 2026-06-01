namespace BolaoCopaApp.Domain.ValueObjects;

public record PointsBreakdown
{
    public int GroupPts { get; init; }
    public int KnockoutPts { get; init; }
    public int SpecialPts { get; init; }
    
    public int Total => GroupPts + KnockoutPts + SpecialPts;

    public PointsBreakdown(int groupPts = 0, int knockoutPts = 0, int specialPts = 0)
    {
        GroupPts = groupPts;
        KnockoutPts = knockoutPts;
        SpecialPts = specialPts;
    }
}
