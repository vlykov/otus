using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Otus.Homework.Delivery.Domain;

namespace Otus.Homework.Delivery.Infrastructure.Persistence.Configurations;

public sealed class DeliveryConfiguration : IEntityTypeConfiguration<Courier>
{
    public void Configure(EntityTypeBuilder<Courier> builder)
    {
        builder.HasKey(_ => _.Id);
        builder.Property(_ => _.Id)
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Property(_ => _.OrderId)
            .IsRequired();

        builder.Property(_ => _.Product)
            .IsRequired();

        builder.Property(_ => _.Status)
            .IsRequired();
    }
}
