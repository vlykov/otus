using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Otus.Homework.Warehouse.Domain;

namespace Otus.Homework.Warehouse.Infrastructure.Persistence.Configurations;

public sealed class ReservationnConfiguration : IEntityTypeConfiguration<Reservation>
{
    public void Configure(EntityTypeBuilder<Reservation> builder)
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
