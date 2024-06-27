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

        modelBuilder.AddInboxStateEntity(_ => _.ToTable("InboxState", "transport"));
        modelBuilder.AddOutboxMessageEntity(_ => _.ToTable("OutboxMessage", "transport"));
        modelBuilder.AddOutboxStateEntity(_ => _.ToTable("OutboxState", "transport"));
    }
}