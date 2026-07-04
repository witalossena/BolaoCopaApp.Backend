using BolaoCopaApp.Domain.Enums;
using BolaoCopaApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BolaoCopaApp.API.Services;

public class MatchAutoLockService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<MatchAutoLockService> _logger;

    public MatchAutoLockService(IServiceScopeFactory scopeFactory, ILogger<MatchAutoLockService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await LockDueMatchesAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in MatchAutoLockService");
            }

            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }

    private async Task LockDueMatchesAsync(CancellationToken ct)
    {
        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<BolaoDbContext>();

        var now = DateTime.UtcNow;
        var cutoff = now.AddMinutes(10);
        _logger.LogDebug("AutoLock tick: UtcNow={Now:u}, cutoff={Cutoff:u}", now, cutoff);

        var matches = await context.Matches
            .Where(m => m.Status == MatchStatus.Open && m.MatchDate <= cutoff)
            .ToListAsync(ct);

        _logger.LogDebug("AutoLock found {Count} matches to lock", matches.Count);
        if (matches.Count == 0) return;

        foreach (var match in matches)
        {
            match.Status = MatchStatus.Locked;
            _logger.LogInformation("Auto-locked match {ExternalId} (kickoff {MatchDate:u})", match.ExternalId, match.MatchDate);
        }

        await context.SaveChangesAsync(ct);
    }
}
