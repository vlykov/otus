using Microsoft.EntityFrameworkCore;
using Otus.Homework.Warehouse.Domain;

namespace Otus.Homework.Warehouse.Infrastructure.Persistence;

public class WarehouseContext : DbContext
{
    public WarehouseContext(DbContextOptions<WarehouseContext> options)
    : base(options) { }

    public DbSet<Reservation> ProductReservations { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) => modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
}