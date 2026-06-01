using BolaoCopaApp.Domain.Entities;

namespace BolaoCopaApp.Domain.Interfaces.Repositories;

public interface ITournamentRepository
{
    Task<Tournament?> GetActiveTournamentAsync(CancellationToken cancellationToken = default);
    Task AddAsync(Tournament tournament, CancellationToken cancellationToken = default);
    void Update(Tournament tournament);
}
