using Microsoft.EntityFrameworkCore;
using Otus.Homework.Billing.Domain;

namespace Otus.Homework.Billing.Infrastructure.Persistence;

public class PaymentContext : DbContext
{
    public PaymentContext(DbContextOptions<PaymentContext> options)
    : base(options) { }

    public DbSet<Payment> Payments { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) => modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
}