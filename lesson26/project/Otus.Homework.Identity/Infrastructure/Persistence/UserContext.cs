using Microsoft.EntityFrameworkCore;
using Otus.Homework.Identity.Domain;

namespace Otus.Homework.Identity.Infrastructure.Persistence;

class UserContext : DbContext
{
    public UserContext(DbContextOptions<UserContext> options)
    : base(options) { }

    public DbSet<User> Users => Set<User>(); // { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}