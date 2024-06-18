using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Otus.Msa.Profile.Api.Infrastructure.Persistence;

internal class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<UserProfileContext>
{
    public UserProfileContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("appsettings.Development.json")
           .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection");

        var builder = new DbContextOptionsBuilder<UserProfileContext>()
            .UseNpgsql(connectionString);

        return new UserProfileContext(builder.Options);
    }
}
