using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Otus.Homework.Identity.Domain;

namespace Otus.Homework.Identity.Infrastructure.Persistence.Configurations;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(user => user.Id);
        builder.Property(user => user.Id)
            .IsRequired()
            .UseIdentityAlwaysColumn();

        builder.Property(user => user.Login)
            .IsRequired();

        builder.Property(user => user.Password)
            .IsRequired();

        builder.Property(user => user.Email)
            .IsRequired();

        builder.HasIndex(user => user.Login)
            .IsUnique();

        builder.HasIndex(user => user.Email)
            .IsUnique();
    }
}
