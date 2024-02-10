
using Domain.Core.ValueObjects;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

internal class HorseConfiguration : IEntityTypeConfiguration<Horse>
{
    public void Configure(EntityTypeBuilder<Horse> builder)
    {
        builder.HasKey(c => c.Id);

         builder.OwnsOne(horse => horse.Name, nameBuilder =>
        {
            nameBuilder.WithOwner();

            nameBuilder.Property(name => name.Value)
                .HasColumnName(nameof(Horse.Name))
                .HasMaxLength(Name.MaxLength)
                .IsRequired();
        });

        builder.Property(c => c.CreatedOnUtc)
            .IsRequired();

        builder.HasIndex(c => c.Name).IsUnique();
    }
}
