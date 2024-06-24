using Microsoft.EntityFrameworkCore;
using Otus.Homework.Common.Idempotency;
using Otus.Homework.Orders.Domain;

namespace Otus.Homework.Orders.Infrastructure.Persistence;

public class OrderContext : DbContext
{
    public OrderContext(DbContextOptions<OrderContext> options)
    : base(options) { }

    public DbSet<Order> Orders { get; init; }
    public DbSet<ClientRequest> ClientRequests { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) => modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
}