using BolaoCopaApp.Domain.Entities;

namespace BolaoCopaApp.Domain.Interfaces.Repositories;

public interface ISpecialPredictionRepository
{
    Task<SpecialPrediction?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task AddAsync(SpecialPrediction prediction, CancellationToken cancellationToken = default);
    void Update(SpecialPrediction prediction);
}
