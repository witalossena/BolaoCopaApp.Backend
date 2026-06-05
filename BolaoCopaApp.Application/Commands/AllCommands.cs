using BolaoCopaApp.Application.DTOs;
using MediatR;

namespace BolaoCopaApp.Application.Commands;

public record RegisterUserCommand(RegisterRequest Request) : IRequest<AuthResponse>;
public record LoginCommand(LoginRequest Request) : IRequest<AuthResponse>;

public record SubmitPredictionCommand(Guid UserId, string MatchId, int HomeScore, int AwayScore) : IRequest<bool>;
public record SubmitGroupRankCommand(Guid UserId, string Group, string FirstTeam, string SecondTeam) : IRequest<bool>;
public record SubmitSpecialPredictionCommand(Guid UserId, SpecialPredictionDto Prediction) : IRequest<bool>;
public record SubmitKnockoutPredictionCommand(Guid UserId, string MatchId, string WinnerTeam) : IRequest<bool>;
public record ClearKnockoutPredictionsCommand(Guid UserId) : IRequest<bool>;

public record RegisterMatchResultCommand(string MatchId, int HomeScore, int AwayScore) : IRequest<bool>;
public record CalculateAllScoresCommand() : IRequest<bool>;
public record ToggleUserPaymentCommand(Guid UserId, bool IsPaid) : IRequest<bool>;
public record LockMatchCommand(string MatchId, bool IsLocked) : IRequest<bool>;
public record UpdateMatchTeamsCommand(string MatchId, string HomeTeam, string AwayTeam) : IRequest<bool>;
public record SeedMatchesCommand() : IRequest<bool>;
