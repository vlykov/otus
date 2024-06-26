using InternetShop.Billing.Domain;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace InternetShop.Billing.Infrastructure.Persistence;

public class CoreDbContext : DbContext
{
    public CoreDbContext(DbContextOptions<CoreDbContext> options)
    : base(options) { }

    public DbSet<Account> Accounts { get; init; }
    public DbSet<Payment> Payments { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("microservice");
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);

        modelBuilder.AddInboxStateEntity();
        modelBuilder.AddOutboxMessageEntity();
        modelBuilder.AddOutboxStateEntity();
    }
}