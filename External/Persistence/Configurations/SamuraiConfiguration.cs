
using Domain.Core.ValueObjects;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

internal class SamuraiConfiguration : IEntityTypeConfiguration<Samurai>
{
    public void Configure(EntityTypeBuilder<Samurai> builder)
    {
        builder.HasKey(c => c.Id);

        builder.OwnsOne(samurai => samurai.Name, nameBuilder =>
        {
            nameBuilder.WithOwner();

            nameBuilder.Property(name => name.Value)
                .HasColumnName(nameof(Samurai.Name))
                .HasMaxLength(Name.MaxLength)
                .IsRequired();
        });

        builder.Property(c => c.CreatedOnUtc)
            .IsRequired();

        builder.HasIndex(c => c.Name).IsUnique();
    }
}
