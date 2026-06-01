using BolaoCopaApp.Domain.Entities;

namespace BolaoCopaApp.Domain.Interfaces.Repositories;

public interface IPredictionRepository
{
    Task<Prediction?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Prediction?> GetByUserAndMatchAsync(Guid userId, Guid matchId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Prediction>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Prediction>> GetByMatchIdAsync(Guid matchId, CancellationToken cancellationToken = default);
    Task AddAsync(Prediction prediction, CancellationToken cancellationToken = default);
    void Update(Prediction prediction);
}
