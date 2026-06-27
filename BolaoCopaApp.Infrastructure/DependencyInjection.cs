using BolaoCopaApp.Domain.Interfaces;
using BolaoCopaApp.Domain.Interfaces.Repositories;
using BolaoCopaApp.Infrastructure.Persistence;
using BolaoCopaApp.Infrastructure.Repositories;
using BolaoCopaApp.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BolaoCopaApp.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<BolaoDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(BolaoDbContext).Assembly.FullName)));

        services.AddScoped<IUnitOfWork>(provider => provider.GetRequiredService<BolaoDbContext>());
        
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IMatchRepository, MatchRepository>();
        services.AddScoped<IPredictionRepository, PredictionRepository>();
        services.AddScoped<IGroupRankPredictionRepository, GroupRankPredictionRepository>();
        services.AddScoped<ISpecialPredictionRepository, SpecialPredictionRepository>();
        services.AddScoped<IKnockoutPredictionRepository, KnockoutPredictionRepository>();
        services.AddScoped<ITournamentRepository, TournamentRepository>();
        services.AddScoped<IGroupResultRepository, GroupResultRepository>();
        services.AddScoped<BolaoCopaApp.Application.Interfaces.IJwtService, BolaoCopaApp.Infrastructure.Services.JwtService>();
        services.AddScoped<BolaoCopaApp.Application.Interfaces.IPaymentService, BolaoCopaApp.Infrastructure.Services.MercadoPagoService>();
        services.AddHostedService<MatchAutoLockService>();

        return services;
    }
}
