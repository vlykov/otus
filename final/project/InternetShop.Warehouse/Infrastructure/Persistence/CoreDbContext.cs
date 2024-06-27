using Microsoft.EntityFrameworkCore;
using InternetShop.Warehouse.Domain;
using MassTransit;

namespace InternetShop.Warehouse.Infrastructure.Persistence;

public class CoreDbContext : DbContext
{
    public CoreDbContext(DbContextOptions<CoreDbContext> options)
    : base(options) { }

    public DbSet<Product> Products { get; init; }
    public DbSet<Reservation> ProductReservations { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("microservice");
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);

        modelBuilder.AddInboxStateEntity(_ => _.ToTable("InboxState", "transport"));
        modelBuilder.AddOutboxMessageEntity(_ => _.ToTable("OutboxMessage", "transport"));
        modelBuilder.AddOutboxStateEntity(_ => _.ToTable("OutboxState", "transport"));
    }
}