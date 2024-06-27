using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InternetShop.Delivery.Infrastructure.Persistence.Configurations;

public sealed class DeliveryConfiguration : IEntityTypeConfiguration<Domain.Delivery>
{
    public void Configure(EntityTypeBuilder<Domain.Delivery> builder)
    {
        builder.HasKey(_ => _.Id);
        builder.Property(_ => _.Id)
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Property(_ => _.OrderId)
            .IsRequired();

        builder.Property(_ => _.CourierId)
            .IsRequired();

        builder.Property(_ => _.Status)
            .IsRequired();
    }
}
