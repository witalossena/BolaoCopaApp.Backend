using BolaoCopaApp.Domain.Enums;

namespace BolaoCopaApp.Domain.Entities;

public class Tournament
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public int Season { get; set; } = 2026;
    public bool IsActive { get; set; } = true;
    public TournamentPhase CurrentPhase { get; set; } = TournamentPhase.PreTournament;
    public decimal PrizePool { get; set; } = 0;
    public bool ArePredictionsLocked { get; set; } = false;
}
