using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BolaoCopaApp.Infrastructure.Persistence;

public class BolaoDbContextFactory : IDesignTimeDbContextFactory<BolaoDbContext>
{
    public BolaoDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<BolaoDbContext>();
        optionsBuilder.UseNpgsql("Host=187.45.255.12;Port=54322;Database=CopaBolao;Username=copa_bolao_user;Password=XkYuredgcmiu8QaK2506");

        return new BolaoDbContext(optionsBuilder.Options);
    }
}
