using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using InternetShop.Warehouse.Domain;

namespace InternetShop.Warehouse.Infrastructure.Persistence.Configurations;

public sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(_ => _.Id);
        builder.Property(_ => _.Id)
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Property(_ => _.Name)
            .IsRequired();

        builder.HasIndex(_ => _.Name)
            .IsUnique();

        builder.Property(_ => _.Quantity)
            .IsRequired();
    }
}
