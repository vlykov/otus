using Microsoft.EntityFrameworkCore;
using Otus.Msa.Profile.Api.Domain;

namespace Otus.Msa.Profile.Api.Infrastructure.Persistence;

class UserProfileContext : DbContext
{
    public UserProfileContext(DbContextOptions<UserProfileContext> options)
    : base(options) { }

    public DbSet<UserProfile> UserProfiles => Set<UserProfile>(); // { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}