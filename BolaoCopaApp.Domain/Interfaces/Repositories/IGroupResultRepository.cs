using BolaoCopaApp.Domain.Entities;

namespace BolaoCopaApp.Domain.Interfaces.Repositories;

public interface IGroupResultRepository
{
    Task<GroupResult?> GetByGroupAsync(string group, CancellationToken ct = default);
    Task<IEnumerable<GroupResult>> GetAllAsync(CancellationToken ct = default);
    Task AddAsync(GroupResult result, CancellationToken ct = default);
    void Update(GroupResult result);
    void Remove(GroupResult result);
}
