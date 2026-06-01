using BolaoCopaApp.Domain.Entities;
using BolaoCopaApp.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BolaoCopaApp.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);
        
        builder.Property(u => u.Email)
            .HasConversion(e => e.Value, v => new Email(v))
            .IsRequired()
            .HasMaxLength(255);
        builder.HasIndex(u => u.Email).IsUnique();

        builder.Property(u => u.Handle)
            .HasConversion(h => h.Value, v => new Handle(v))
            .IsRequired()
            .HasMaxLength(20);
        builder.HasIndex(u => u.Handle).IsUnique();

        builder.Property(u => u.Role).HasConversion<string>();
    }
}

public class MatchConfiguration : IEntityTypeConfiguration<Match>
{
    public void Configure(EntityTypeBuilder<Match> builder)
    {
        builder.HasKey(m => m.Id);
        
        builder.Property(m => m.ExternalId).IsRequired();
        builder.HasIndex(m => m.ExternalId).IsUnique();
        
        builder.Property(m => m.Status).HasConversion<string>();
        builder.Property(m => m.Round).HasConversion<string>();

        builder.Property(m => m.HomeScore)
            .HasConversion(s => s != null ? s.Value : (int?)null, v => v.HasValue ? new Score(v.Value) : null);
        
        builder.Property(m => m.AwayScore)
            .HasConversion(s => s != null ? s.Value : (int?)null, v => v.HasValue ? new Score(v.Value) : null);
    }
}

public class PredictionConfiguration : IEntityTypeConfiguration<Prediction>
{
    public void Configure(EntityTypeBuilder<Prediction> builder)
    {
        builder.HasKey(p => p.Id);
        builder.HasIndex(p => new { p.UserId, p.MatchId }).IsUnique();

        builder.Property(p => p.HomeScore)
            .HasConversion(s => s.Value, v => new Score(v))
            .IsRequired();
            
        builder.Property(p => p.AwayScore)
            .HasConversion(s => s.Value, v => new Score(v))
            .IsRequired();
    }
}

public class GroupRankPredictionConfiguration : IEntityTypeConfiguration<GroupRankPrediction>
{
    public void Configure(EntityTypeBuilder<GroupRankPrediction> builder)
    {
        builder.HasKey(g => g.Id);
        builder.HasIndex(g => new { g.UserId, g.Group }).IsUnique();
    }
}

public class SpecialPredictionConfiguration : IEntityTypeConfiguration<SpecialPrediction>
{
    public void Configure(EntityTypeBuilder<SpecialPrediction> builder)
    {
        builder.HasKey(s => s.Id);
        builder.HasIndex(s => s.UserId).IsUnique(); // 1 per user
    }
}
