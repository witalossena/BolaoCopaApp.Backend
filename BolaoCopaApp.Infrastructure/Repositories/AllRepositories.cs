using BolaoCopaApp.Domain.Entities;
using BolaoCopaApp.Domain.Enums;
using BolaoCopaApp.Domain.Interfaces.Repositories;
using BolaoCopaApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BolaoCopaApp.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly BolaoDbContext _context;
    public UserRepository(BolaoDbContext context) => _context = context;

    public async Task<User?> GetByIdAsync(Guid id, CancellationToken ct = default) => await _context.Users.FindAsync([id], ct);
    public async Task<User?> GetByEmailAsync(string email, CancellationToken ct = default) => await _context.Users.FirstOrDefaultAsync(u => u.Email.Equals(email), ct);
    public async Task<User?> GetByHandleAsync(string handle, CancellationToken ct = default) => await _context.Users.FirstOrDefaultAsync(u => u.Handle.Equals(handle), ct);
    public async Task<IEnumerable<User>> GetAllAsync(CancellationToken ct = default) => await _context.Users.ToListAsync(ct);
    public async Task AddAsync(User user, CancellationToken ct = default) => await _context.Users.AddAsync(user, ct);
    public void Update(User user) => _context.Users.Update(user);
}

public class MatchRepository : IMatchRepository
{
    private readonly BolaoDbContext _context;
    public MatchRepository(BolaoDbContext context) => _context = context;

    public async Task<Match?> GetByIdAsync(Guid id, CancellationToken ct = default) => await _context.Matches.FindAsync([id], ct);
    public async Task<IEnumerable<Match>> GetAllAsync(CancellationToken ct = default) => await _context.Matches.ToListAsync(ct);
    public async Task<IEnumerable<Match>> GetByGroupAsync(string group, CancellationToken ct = default) => await _context.Matches.Where(m => m.Group == group).ToListAsync(ct);
    public async Task<IEnumerable<Match>> GetByRoundAsync(MatchRound round, CancellationToken ct = default) => await _context.Matches.Where(m => m.Round == round).ToListAsync(ct);
    public async Task AddRangeAsync(IEnumerable<Match> matches, CancellationToken ct = default) => await _context.Matches.AddRangeAsync(matches, ct);
    public void Update(Match match) => _context.Matches.Update(match);
}

public class PredictionRepository : IPredictionRepository
{
    private readonly BolaoDbContext _context;
    public PredictionRepository(BolaoDbContext context) => _context = context;

    public async Task<Prediction?> GetByIdAsync(Guid id, CancellationToken ct = default) => await _context.Predictions.FindAsync([id], ct);
    public async Task<Prediction?> GetByUserAndMatchAsync(Guid userId, Guid matchId, CancellationToken ct = default) => await _context.Predictions.FirstOrDefaultAsync(p => p.UserId == userId && p.MatchId == matchId, ct);
    public async Task<IEnumerable<Prediction>> GetByUserIdAsync(Guid userId, CancellationToken ct = default) => await _context.Predictions.Where(p => p.UserId == userId).ToListAsync(ct);
    public async Task<IEnumerable<Prediction>> GetByMatchIdAsync(Guid matchId, CancellationToken ct = default) => await _context.Predictions.Where(p => p.MatchId == matchId).ToListAsync(ct);
    public async Task AddAsync(Prediction prediction, CancellationToken ct = default) => await _context.Predictions.AddAsync(prediction, ct);
    public void Update(Prediction prediction) => _context.Predictions.Update(prediction);
}

public class GroupRankPredictionRepository : IGroupRankPredictionRepository
{
    private readonly BolaoDbContext _context;
    public GroupRankPredictionRepository(BolaoDbContext context) => _context = context;

    public async Task<GroupRankPrediction?> GetByUserAndGroupAsync(Guid userId, string group, CancellationToken ct = default) => await _context.GroupRankPredictions.FirstOrDefaultAsync(g => g.UserId == userId && g.Group == group, ct);
    public async Task<IEnumerable<GroupRankPrediction>> GetByUserIdAsync(Guid userId, CancellationToken ct = default) => await _context.GroupRankPredictions.Where(g => g.UserId == userId).ToListAsync(ct);
    public async Task AddAsync(GroupRankPrediction prediction, CancellationToken ct = default) => await _context.GroupRankPredictions.AddAsync(prediction, ct);
    public void Update(GroupRankPrediction prediction) => _context.GroupRankPredictions.Update(prediction);
}

public class SpecialPredictionRepository : ISpecialPredictionRepository
{
    private readonly BolaoDbContext _context;
    public SpecialPredictionRepository(BolaoDbContext context) => _context = context;

    public async Task<SpecialPrediction?> GetByUserIdAsync(Guid userId, CancellationToken ct = default) => await _context.SpecialPredictions.FirstOrDefaultAsync(s => s.UserId == userId, ct);
    public async Task AddAsync(SpecialPrediction prediction, CancellationToken ct = default) => await _context.SpecialPredictions.AddAsync(prediction, ct);
    public void Update(SpecialPrediction prediction) => _context.SpecialPredictions.Update(prediction);
}

public class KnockoutPredictionRepository : IKnockoutPredictionRepository
{
    private readonly BolaoDbContext _context;
    public KnockoutPredictionRepository(BolaoDbContext context) => _context = context;

    public async Task<KnockoutPrediction?> GetByUserAndMatchAsync(Guid userId, Guid matchId, CancellationToken ct = default) => await _context.KnockoutPredictions.FirstOrDefaultAsync(k => k.UserId == userId && k.MatchId == matchId, ct);
    public async Task<IEnumerable<KnockoutPrediction>> GetByUserIdAsync(Guid userId, CancellationToken ct = default) => await _context.KnockoutPredictions.Where(k => k.UserId == userId).ToListAsync(ct);
    public async Task AddAsync(KnockoutPrediction prediction, CancellationToken ct = default) => await _context.KnockoutPredictions.AddAsync(prediction, ct);
    public void Update(KnockoutPrediction prediction) => _context.KnockoutPredictions.Update(prediction);
}

public class TournamentRepository : ITournamentRepository
{
    private readonly BolaoDbContext _context;
    public TournamentRepository(BolaoDbContext context) => _context = context;

    public async Task<Tournament?> GetActiveTournamentAsync(CancellationToken ct = default) => await _context.Tournaments.FirstOrDefaultAsync(t => t.IsActive, ct);
    public async Task AddAsync(Tournament tournament, CancellationToken ct = default) => await _context.Tournaments.AddAsync(tournament, ct);
    public void Update(Tournament tournament) => _context.Tournaments.Update(tournament);
}
