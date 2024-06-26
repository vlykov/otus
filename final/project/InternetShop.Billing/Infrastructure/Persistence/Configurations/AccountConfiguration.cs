using InternetShop.Billing.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Otus.Homework.Billing.Infrastructure.Persistence.Configurations;

public sealed class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.HasKey(_ => _.Id);
        builder.Property(_ => _.Id)
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Property(_ => _.UserId)
            .IsRequired();

        builder.HasIndex(_ => _.UserId)
            .IsUnique();

        builder.Property(_ => _.Balance)
            .IsRequired();
    }
}
