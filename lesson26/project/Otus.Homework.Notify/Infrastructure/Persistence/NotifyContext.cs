using Microsoft.EntityFrameworkCore;
using Otus.Homework.Notify.Domain;

namespace Otus.Homework.Notify.Infrastructure.Persistence;

public class NotifyContext : DbContext
{
    public NotifyContext(DbContextOptions<NotifyContext> options)
    : base(options) { }

    public DbSet<Notification> Notifications { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) => modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
}