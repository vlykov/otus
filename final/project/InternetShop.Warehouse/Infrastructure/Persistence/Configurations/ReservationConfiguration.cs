using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using InternetShop.Warehouse.Domain;

namespace InternetShop.Warehouse.Infrastructure.Persistence.Configurations;

public sealed class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
{
    public void Configure(EntityTypeBuilder<Reservation> builder)
    {
        builder.HasKey(_ => _.Id);
        builder.Property(_ => _.Id)
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Property(_ => _.OrderId)
            .IsRequired();

        builder.Property(_ => _.ProductId)
            .IsRequired();

        builder.Property(_ => _.Quantity)
            .IsRequired();

        builder.Property(_ => _.Status)
            .IsRequired();
    }
}
