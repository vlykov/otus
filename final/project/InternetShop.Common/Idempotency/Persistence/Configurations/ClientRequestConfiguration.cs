using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InternetShop.Common.Idempotency.Persistence.Configurations;

public sealed class ClientRequestConfiguration : IEntityTypeConfiguration<ClientRequest>
{
    public void Configure(EntityTypeBuilder<ClientRequest> builder)
    {
        builder.HasKey(_ => _.Id);
        builder.Property(_ => _.Id)
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(_ => _.Name)
            .IsRequired();

        builder.Property(_ => _.CreatedAt)
            .IsRequired();
    }
}
