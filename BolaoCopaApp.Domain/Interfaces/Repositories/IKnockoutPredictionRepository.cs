using BolaoCopaApp.Domain.Entities;

namespace BolaoCopaApp.Domain.Interfaces.Repositories;

public interface IKnockoutPredictionRepository
{
    Task<KnockoutPrediction?> GetByUserAndMatchAsync(Guid userId, Guid matchId, CancellationToken cancellationToken = default);
    Task<IEnumerable<KnockoutPrediction>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<KnockoutPrediction>> GetByMatchIdAsync(Guid matchId, CancellationToken cancellationToken = default);
    Task AddAsync(KnockoutPrediction prediction, CancellationToken cancellationToken = default);
    void Update(KnockoutPrediction prediction);
    void RemoveRange(IEnumerable<KnockoutPrediction> predictions);
}
