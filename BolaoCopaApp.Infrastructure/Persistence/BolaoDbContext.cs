using BolaoCopaApp.Domain.Entities;
using BolaoCopaApp.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace BolaoCopaApp.Infrastructure.Persistence;

public class BolaoDbContext : DbContext, IUnitOfWork
{
    public BolaoDbContext(DbContextOptions<BolaoDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Match> Matches => Set<Match>();
    public DbSet<Prediction> Predictions => Set<Prediction>();
    public DbSet<GroupRankPrediction> GroupRankPredictions => Set<GroupRankPrediction>();
    public DbSet<SpecialPrediction> SpecialPredictions => Set<SpecialPrediction>();
    public DbSet<KnockoutPrediction> KnockoutPredictions => Set<KnockoutPrediction>();
    public DbSet<Tournament> Tournaments => Set<Tournament>();
    public DbSet<GroupResult> GroupResults => Set<GroupResult>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}
