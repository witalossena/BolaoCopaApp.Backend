using BolaoCopaApp.Domain.Entities;

namespace BolaoCopaApp.Domain.Interfaces.Repositories;

public interface IGroupRankPredictionRepository
{
    Task<GroupRankPrediction?> GetByUserAndGroupAsync(Guid userId, string group, CancellationToken cancellationToken = default);
    Task<IEnumerable<GroupRankPrediction>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task AddAsync(GroupRankPrediction prediction, CancellationToken cancellationToken = default);
    void Update(GroupRankPrediction prediction);
}
