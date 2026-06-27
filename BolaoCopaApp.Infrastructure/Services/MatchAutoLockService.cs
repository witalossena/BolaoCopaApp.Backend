using BolaoCopaApp.Domain.Enums;
using BolaoCopaApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BolaoCopaApp.Infrastructure.Services;

public class MatchAutoLockService : BackgroundService
{
    private readonly IServiceProvider _services;
    private readonly ILogger<MatchAutoLockService> _logger;

    public MatchAutoLockService(IServiceProvider services, ILogger<MatchAutoLockService> logger)
    {
        _services = services;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await LockDueMatchesAsync();
            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }

    private async Task LockDueMatchesAsync()
    {
        using var scope = _services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<BolaoDbContext>();

        var cutoff = DateTime.UtcNow.AddMinutes(10);

        var tolock = await db.Matches
            .Where(m => m.Status == MatchStatus.Open && m.MatchDate <= cutoff)
            .ToListAsync();

        if (tolock.Count == 0) return;

        foreach (var m in tolock)
            m.Status = MatchStatus.Locked;

        await db.SaveChangesAsync();
        _logger.LogInformation("Auto-locked {Count} matches at {Time}", tolock.Count, DateTime.UtcNow);
    }
}
