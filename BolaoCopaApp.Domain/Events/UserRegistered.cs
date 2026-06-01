using MediatR;

namespace BolaoCopaApp.Domain.Events;

public record UserRegistered(Guid UserId) : INotification;
