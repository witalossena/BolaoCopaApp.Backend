using Microsoft.Extensions.DependencyInjection;
using BolaoCopaApp.Domain.Services;
using FluentValidation;
using System.Reflection;

namespace BolaoCopaApp.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        
        // Domain services
        services.AddScoped<ScoringService>();
        services.AddScoped<PredictionValidationService>();

        return services;
    }
}
