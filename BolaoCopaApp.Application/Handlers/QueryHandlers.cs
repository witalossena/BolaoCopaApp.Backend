using BolaoCopaApp.Application.Queries;
using BolaoCopaApp.Application.DTOs;
using BolaoCopaApp.Domain.Interfaces.Repositories;
using MediatR;

namespace BolaoCopaApp.Application.Handlers;

public class QueryHandlers :
    IRequestHandler<GetMatchesQuery, IEnumerable<MatchDto>>,
    IRequestHandler<GetRankingQuery, IEnumerable<RankingEntryDto>>,
    IRequestHandler<GetAdminStatsQuery, AdminStatsDto>,
    IRequestHandler<GetUsersAdminQuery, IEnumerable<AdminUserDto>>
{
    private readonly IMatchRepository _matchRepo;
    private readonly IUserRepository _userRepo;
    private readonly IPredictionRepository _predictionRepo;
    private readonly IGroupRankPredictionRepository _groupRankRepo;
    private readonly ISpecialPredictionRepository _specialRepo;
    private readonly IKnockoutPredictionRepository _knockoutRepo;

    public QueryHandlers(
        IMatchRepository matchRepo, 
        IUserRepository userRepo,
        IPredictionRepository predictionRepo,
        IGroupRankPredictionRepository groupRankRepo,
        ISpecialPredictionRepository specialRepo,
        IKnockoutPredictionRepository knockoutRepo)
    {
        _matchRepo = matchRepo;
        _userRepo = userRepo;
        _predictionRepo = predictionRepo;
        _groupRankRepo = groupRankRepo;
        _specialRepo = specialRepo;
        _knockoutRepo = knockoutRepo;
    }

    public async Task<AdminStatsDto> Handle(GetAdminStatsQuery request, CancellationToken cancellationToken)
    {
        var users = await _userRepo.GetAllAsync(cancellationToken);
        var matches = await _matchRepo.GetAllAsync(cancellationToken);

        int totalUsers = users.Count();
        int paidCount = users.Count(u => u.IsPaid);
        int pendingCount = totalUsers - paidCount;
        int totalMatches = matches.Count();

        return new AdminStatsDto(totalUsers, paidCount, pendingCount, totalMatches);
    }

    public async Task<IEnumerable<AdminUserDto>> Handle(GetUsersAdminQuery request, CancellationToken cancellationToken)
    {
        var users = await _userRepo.GetAllAsync(cancellationToken);
        var result = new List<AdminUserDto>();

        foreach (var user in users)
        {
            var matchPredictions = await _predictionRepo.GetByUserIdAsync(user.Id, cancellationToken);
            var groupPredictions = await _groupRankRepo.GetByUserIdAsync(user.Id, cancellationToken);
            var specialPrediction = await _specialRepo.GetByUserIdAsync(user.Id, cancellationToken);
            var knockoutPredictions = await _knockoutRepo.GetByUserIdAsync(user.Id, cancellationToken);

            int totalPts = matchPredictions.Sum(p => p.Points) + 
                          groupPredictions.Sum(g => g.Points) + 
                          (specialPrediction?.Points ?? 0) + 
                          knockoutPredictions.Sum(k => k.Points);

            result.Add(new AdminUserDto(user.Id, user.Name, user.Handle.Value, totalPts, user.IsPaid));
        }

        return result.OrderBy(u => u.Name);
    }

    public async Task<IEnumerable<RankingEntryDto>> Handle(GetRankingQuery request, CancellationToken cancellationToken)
    {
        var users = await _userRepo.GetAllAsync(cancellationToken);
        var ranking = new List<RankingEntryDto>();

        foreach (var user in users)
        {
            var matchPredictions = await _predictionRepo.GetByUserIdAsync(user.Id, cancellationToken);
            var groupPredictions = await _groupRankRepo.GetByUserIdAsync(user.Id, cancellationToken);
            var specialPrediction = await _specialRepo.GetByUserIdAsync(user.Id, cancellationToken);
            var knockoutPredictions = await _knockoutRepo.GetByUserIdAsync(user.Id, cancellationToken);

            int groupPts = matchPredictions.Sum(p => p.Points) + groupPredictions.Sum(g => g.Points);
            int specialPts = (specialPrediction?.Points ?? 0) + knockoutPredictions.Sum(k => k.Points);
            int total = groupPts + specialPts;

            ranking.Add(new RankingEntryDto(0, user.Name, user.Handle.Value, groupPts, specialPts, total, user.IsPaid));
        }

        return ranking.OrderByDescending(r => r.Total)
                      .Select((r, i) => r with { Position = i + 1 });
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
            m.ExternalId,
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
