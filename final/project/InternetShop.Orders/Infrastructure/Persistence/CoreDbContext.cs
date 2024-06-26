using Microsoft.EntityFrameworkCore;
using InternetShop.Orders.Domain;
using MassTransit;
using InternetShop.Common.Idempotency;

namespace InternetShop.Orders.Infrastructure.Persistence;

public class CoreDbContext : DbContext
{
    public CoreDbContext(DbContextOptions<CoreDbContext> options)
    : base(options) { }

    public DbSet<Order> Orders { get; init; }
    public DbSet<ClientRequest> ClientRequests { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("microservice");
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);

        modelBuilder.AddInboxStateEntity();
        modelBuilder.AddOutboxMessageEntity();
        modelBuilder.AddOutboxStateEntity();
    }
}