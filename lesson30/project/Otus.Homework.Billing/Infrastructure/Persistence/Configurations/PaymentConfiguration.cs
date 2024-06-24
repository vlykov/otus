using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Otus.Homework.Billing.Domain;

namespace Otus.Homework.Billing.Infrastructure.Persistence.Configurations;

public sealed class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.HasKey(_ => _.Id);
        builder.Property(_ => _.Id)
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Property(_ => _.OrderId)
            .IsRequired();

        builder.Property(_ => _.Product)
            .IsRequired();

        builder.Property(_ => _.TotalPrice)
            .IsRequired();

        builder.Property(_ => _.Status)
            .IsRequired();
    }
}
