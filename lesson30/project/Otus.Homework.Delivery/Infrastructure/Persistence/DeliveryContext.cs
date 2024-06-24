using Microsoft.EntityFrameworkCore;
using Otus.Homework.Delivery.Domain;

namespace Otus.Homework.Delivery.Infrastructure.Persistence;

public class DeliveryContext : DbContext
{
    public DeliveryContext(DbContextOptions<DeliveryContext> options)
    : base(options) { }

    public DbSet<Courier> Couriers { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) => modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
}