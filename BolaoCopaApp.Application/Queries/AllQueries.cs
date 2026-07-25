using BolaoCopaApp.Application.DTOs;
using MediatR;

namespace BolaoCopaApp.Application.Queries;

public record GetRankingQuery() : IRequest<IEnumerable<RankingEntryDto>>;
public record GetUserPredictionsQuery(Guid UserId) : IRequest<UserPredictionsDto>;
public record GetUserPerformanceQuery(Guid UserId) : IRequest<PerformanceDto>;
public record GetMatchesQuery(string? Group, string? Round) : IRequest<IEnumerable<MatchDto>>;
public record GetUsersAdminQuery() : IRequest<IEnumerable<AdminUserDto>>;
public record GetAdminStatsQuery() : IRequest<AdminStatsDto>;
public record GetUserHistoryQuery(Guid UserId) : IRequest<IEnumerable<PredictionHistoryItemDto>>;
public record GetGroupResultsQuery() : IRequest<IEnumerable<GroupResultSummaryDto>>;
public record GetTournamentInfoQuery() : IRequest<TournamentInfoDto>;
public record GetBetsReportQuery() : IRequest<BetsReportDto>;
