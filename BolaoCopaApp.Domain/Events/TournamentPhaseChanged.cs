using MediatR;
using BolaoCopaApp.Domain.Enums;

namespace BolaoCopaApp.Domain.Events;

public record TournamentPhaseChanged(Guid TournamentId, TournamentPhase NewPhase) : INotification;
