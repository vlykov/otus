using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using InternetShop.Notify.Domain;

namespace InternetShop.Notify.Infrastructure.Persistence.Configurations;

public sealed class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.HasKey(_ => _.Id);
        builder.Property(_ => _.Id)
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Property(_ => _.UserId)
            .IsRequired();

        builder.Property(_ => _.Message)
            .IsRequired();
    }
}
