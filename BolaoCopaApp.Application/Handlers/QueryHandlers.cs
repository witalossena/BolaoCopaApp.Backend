using BolaoCopaApp.Application.Queries;
using BolaoCopaApp.Application.DTOs;
using BolaoCopaApp.Domain.Interfaces.Repositories;
using MediatR;

namespace BolaoCopaApp.Application.Handlers;

public class QueryHandlers :
    IRequestHandler<GetMatchesQuery, IEnumerable<MatchDto>>
{
    private readonly IMatchRepository _matchRepo;

    public QueryHandlers(IMatchRepository matchRepo)
    {
        _matchRepo = matchRepo;
    }

    public async Task<IEnumerable<MatchDto>> Handle(GetMatchesQuery request, CancellationToken cancellationToken)
    {
        var matches = await _matchRepo.GetAllAsync(cancellationToken);
        
        if (!string.IsNullOrEmpty(request.Group))
        {
            matches = matches.Where(m => m.Group == request.Group);
        }
        if (!string.IsNullOrEmpty(request.Round))
        {
            matches = matches.Where(m => m.Round.ToString() == request.Round);
        }

        return matches.Select(m => new MatchDto(
            m.Id.ToString(),
            m.HomeTeam,
            m.AwayTeam,
            m.Group ?? "",
            m.Round.ToString(),
            m.MatchDate,
            m.Status.ToString(),
            m.HomeScore?.Value,
            m.AwayScore?.Value
        )).OrderBy(m => m.MatchDate);
    }
}
