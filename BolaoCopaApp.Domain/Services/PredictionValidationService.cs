using BolaoCopaApp.Domain.Entities;
using BolaoCopaApp.Domain.Enums;

namespace BolaoCopaApp.Domain.Services;

public class PredictionValidationService
{
    public bool IsMatchPredictionAllowed(Match match)
    {
        return match.Status == MatchStatus.Open;
    }

    public bool IsGroupRankPredictionAllowed(IEnumerable<Match> groupMatches)
    {
        // Allowed if no match in the group has started yet
        return groupMatches.All(m => m.Status == MatchStatus.Open);
    }

    public bool IsKnockoutPredictionAllowed(Match knockoutMatch, TournamentPhase currentPhase)
    {
        return knockoutMatch.Status == MatchStatus.Open && currentPhase >= TournamentPhase.KnockoutStage;
    }

    public bool IsSpecialPredictionAllowed(TournamentPhase currentPhase)
    {
        return currentPhase == TournamentPhase.PreTournament;
    }
}
