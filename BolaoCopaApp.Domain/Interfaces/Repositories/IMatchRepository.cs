using BolaoCopaApp.Domain.Entities;
using BolaoCopaApp.Domain.Enums;

namespace BolaoCopaApp.Domain.Interfaces.Repositories;

public interface IMatchRepository
{
    Task<Match?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Match>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Match>> GetByGroupAsync(string group, CancellationToken cancellationToken = default);
    Task<IEnumerable<Match>> GetByRoundAsync(MatchRound round, CancellationToken cancellationToken = default);
    Task AddRangeAsync(IEnumerable<Match> matches, CancellationToken cancellationToken = default);
    void Update(Match match);
}
