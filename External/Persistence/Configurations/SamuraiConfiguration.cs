
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

internal class SamuraiConfiguration : IEntityTypeConfiguration<Samurai>
{
    public void Configure(EntityTypeBuilder<Samurai> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(c => c.CreatedAt)
            .IsRequired();

        builder.HasIndex(c => c.Name).IsUnique();
    }
}
