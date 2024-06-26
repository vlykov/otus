using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace InternetShop.Identity.Infrastructure.Persistence;

internal class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<CoreDbContext>
{
    public CoreDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("appsettings.Development.json")
           .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection");

        var builder = new DbContextOptionsBuilder<CoreDbContext>()
            .UseNpgsql(connectionString);

        return new CoreDbContext(builder.Options);
    }
}
