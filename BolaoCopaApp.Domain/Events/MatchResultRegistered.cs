using MediatR;

namespace BolaoCopaApp.Domain.Events;

public record MatchResultRegistered(Guid MatchId) : INotification;
