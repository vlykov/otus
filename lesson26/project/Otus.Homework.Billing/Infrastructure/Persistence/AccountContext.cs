using Microsoft.EntityFrameworkCore;
using Otus.Homework.Billing.Domain;

namespace Otus.Homework.Billing.Infrastructure.Persistence;

public class AccountContext : DbContext
{
    public AccountContext(DbContextOptions<AccountContext> options)
    : base(options) { }

    public DbSet<Account> Accounts { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) => modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
}