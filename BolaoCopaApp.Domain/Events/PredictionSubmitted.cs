using MediatR;

namespace BolaoCopaApp.Domain.Events;

public record PredictionSubmitted(Guid PredictionId) : INotification;
