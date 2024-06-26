using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using InternetShop.Orders.Domain;

namespace InternetShop.Orders.Infrastructure.Persistence.Configurations;

public sealed class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(_ => _.Id);
        builder.Property(_ => _.Id)
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Property(_ => _.UserId)
            .IsRequired();

        builder.Property(_ => _.Product)
            .IsRequired();

        builder.Property(_ => _.Quantity)
            .IsRequired();
        
        builder.Property(_ => _.TotalPrice)
            .IsRequired();

        builder.Property(_ => _.Status)
            .IsRequired();
    }
}
