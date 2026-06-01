using BolaoCopaApp.Application.DTOs;
using MediatR;

namespace BolaoCopaApp.Application.Queries;

public record GetRankingQuery() : IRequest<IEnumerable<RankingEntryDto>>;
public record GetUserPredictionsQuery(Guid UserId) : IRequest<object>; // Needs a combined DTO or specific
public record GetUserPerformanceQuery(Guid UserId) : IRequest<PerformanceDto>;
public record GetMatchesQuery(string? Group, string? Round) : IRequest<IEnumerable<MatchDto>>;
public record GetUsersAdminQuery() : IRequest<IEnumerable<AdminUserDto>>;
public record GetAdminStatsQuery() : IRequest<AdminStatsDto>;
