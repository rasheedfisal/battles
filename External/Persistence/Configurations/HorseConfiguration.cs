
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

        builder.Property(p => p.Name)
        .HasConversion(
            HorseName => HorseName.Value,
            value => Name.Create(value).Value
        )
        .HasColumnName(nameof(Horse.Name))
        .HasMaxLength(Name.MaxLength)
        .IsRequired();

        // builder.OwnsOne(horse => horse.Name, nameBuilder =>
        // {
        //     nameBuilder.WithOwner();

        //     nameBuilder.Property(name => name.Value)
        //         .HasColumnName(nameof(Horse.Name))
        //         .HasMaxLength(Name.MaxLength)
        //         .IsRequired();
        // });

        builder.Property(c => c.CreatedOnUtc)
            .IsRequired();
        builder.Property(user => user.ModifiedOnUtc);

        builder.Property(user => user.DeletedOnUtc);

        builder.HasIndex(c => c.Name).IsUnique();
    }
}
