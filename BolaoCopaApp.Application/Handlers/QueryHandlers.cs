using BolaoCopaApp.Application.Queries;
using BolaoCopaApp.Application.DTOs;
using BolaoCopaApp.Domain.Interfaces.Repositories;
using MediatR;

namespace BolaoCopaApp.Application.Handlers;

public class QueryHandlers :
    IRequestHandler<GetMatchesQuery, IEnumerable<MatchDto>>,
    IRequestHandler<GetRankingQuery, IEnumerable<RankingEntryDto>>,
    IRequestHandler<GetAdminStatsQuery, AdminStatsDto>,
    IRequestHandler<GetUsersAdminQuery, IEnumerable<AdminUserDto>>,
    IRequestHandler<GetUserPredictionsQuery, UserPredictionsDto>,
    IRequestHandler<GetUserHistoryQuery, IEnumerable<PredictionHistoryItemDto>>,
    IRequestHandler<GetTournamentInfoQuery, TournamentInfoDto>
{
    private readonly IMatchRepository _matchRepo;
    private readonly IUserRepository _userRepo;
    private readonly IPredictionRepository _predictionRepo;
    private readonly IGroupRankPredictionRepository _groupRankRepo;
    private readonly ISpecialPredictionRepository _specialRepo;
    private readonly IKnockoutPredictionRepository _knockoutRepo;
    private readonly ITournamentRepository _tournamentRepo;

    public QueryHandlers(
        IMatchRepository matchRepo,
        IUserRepository userRepo,
        IPredictionRepository predictionRepo,
        IGroupRankPredictionRepository groupRankRepo,
        ISpecialPredictionRepository specialRepo,
        IKnockoutPredictionRepository knockoutRepo,
        ITournamentRepository tournamentRepo)
    {
        _matchRepo = matchRepo;
        _userRepo = userRepo;
        _predictionRepo = predictionRepo;
        _groupRankRepo = groupRankRepo;
        _specialRepo = specialRepo;
        _knockoutRepo = knockoutRepo;
        _tournamentRepo = tournamentRepo;
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

        return result.OrderByDescending(u => u.TotalPts).ThenBy(u => u.Name);
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

    public async Task<UserPredictionsDto> Handle(GetUserPredictionsQuery request, CancellationToken cancellationToken)
    {
        var allMatches = await _matchRepo.GetAllAsync(cancellationToken);
        var matchMap = allMatches.ToDictionary(m => m.Id, m => m.ExternalId);

        var matchPreds = await _predictionRepo.GetByUserIdAsync(request.UserId, cancellationToken);
        var matchDtos = matchPreds
            .Where(p => matchMap.ContainsKey(p.MatchId))
            .Select(p => new MatchPredictionSummaryDto(matchMap[p.MatchId], p.HomeScore, p.AwayScore));

        var groupPreds = await _groupRankRepo.GetByUserIdAsync(request.UserId, cancellationToken);
        var groupDtos = groupPreds.Select(g => new GroupRankSummaryDto(g.Group, g.FirstTeam, g.SecondTeam, g.ThirdTeam, g.FourthTeam, g.Points));

        var knockoutPreds = await _knockoutRepo.GetByUserIdAsync(request.UserId, cancellationToken);
        var knockoutDtos = knockoutPreds
            .Where(k => matchMap.ContainsKey(k.MatchId))
            .Select(k => new KnockoutPredictionSummaryDto(matchMap[k.MatchId], k.WinnerTeam, k.HomeScore, k.AwayScore));

        return new UserPredictionsDto(matchDtos, groupDtos, knockoutDtos);
    }

    public async Task<IEnumerable<PredictionHistoryItemDto>> Handle(GetUserHistoryQuery request, CancellationToken cancellationToken)
    {
        var preds = await _predictionRepo.GetByUserIdAsync(request.UserId, cancellationToken);
        if (!preds.Any()) return Enumerable.Empty<PredictionHistoryItemDto>();

        var predMap = preds.ToDictionary(p => p.MatchId);
        var allMatches = await _matchRepo.GetAllAsync(cancellationToken);

        return allMatches
            .Where(m => m.Status == BolaoCopaApp.Domain.Enums.MatchStatus.Locked
                     && m.HomeScore != null && m.AwayScore != null
                     && predMap.ContainsKey(m.Id))
            .Select(m =>
            {
                var p = predMap[m.Id];
                return new PredictionHistoryItemDto(
                    m.HomeTeam, m.AwayTeam, m.Group ?? "",
                    p.HomeScore, p.AwayScore,
                    m.HomeScore!, m.AwayScore!,
                    p.Points, m.MatchDate);
            })
            .OrderByDescending(x => x.MatchDate);
    }

    public async Task<TournamentInfoDto> Handle(GetTournamentInfoQuery request, CancellationToken cancellationToken)
    {
        var tournament = await _tournamentRepo.GetActiveTournamentAsync(cancellationToken);
        return new TournamentInfoDto(
            tournament?.CurrentPhase.ToString() ?? "GroupStage",
            tournament?.PrizePool ?? 0);
    }
}
