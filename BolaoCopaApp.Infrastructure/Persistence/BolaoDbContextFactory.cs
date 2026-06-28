using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BolaoCopaApp.Infrastructure.Persistence;

public class BolaoDbContextFactory : IDesignTimeDbContextFactory<BolaoDbContext>
{
    public BolaoDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<BolaoDbContext>();
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=CopaBolao;Username=bolao_user;Password=bolao_dev_pass");

        return new BolaoDbContext(optionsBuilder.Options);
    }
}
