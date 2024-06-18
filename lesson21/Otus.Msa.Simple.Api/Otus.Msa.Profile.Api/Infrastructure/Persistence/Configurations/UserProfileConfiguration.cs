using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Otus.Msa.Profile.Api.Domain;

namespace Otus.Msa.Profile.Api.Infrastructure.Persistence.Configurations;

public sealed class UserProfileConfiguration : IEntityTypeConfiguration<UserProfile>
{
    public void Configure(EntityTypeBuilder<UserProfile> builder)
    {
        builder.HasKey(userProfile => userProfile.Id);
        builder.Property(userProfile => userProfile.Id)
            .IsRequired()
            .ValueGeneratedNever();
    }
}
