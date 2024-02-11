
using Domain.Core.ValueObjects;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

internal class BattleConfiguration : IEntityTypeConfiguration<Battle>
{
    public void Configure(EntityTypeBuilder<Battle> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(p => p.Name)
        .HasConversion(
            BattleName => BattleName.Value,
            value => Name.Create(value).Value
        )
        .HasColumnName(nameof(Battle.Name))
        .HasMaxLength(Name.MaxLength)
        .IsRequired();

        // builder.OwnsOne(battle => battle.Name, nameBuilder =>
        // {
        //     nameBuilder.WithOwner();

        //     nameBuilder.Property(name => name.Value)
        //         .HasColumnName(nameof(Battle.Name))
        //         .HasMaxLength(Name.MaxLength)
        //         .IsRequired();
                
        // });

        builder.Property(c => c.CreatedOnUtc)
            .IsRequired();
        
        builder.Property(user => user.ModifiedOnUtc);

        builder.Property(user => user.DeletedOnUtc);

        builder.HasIndex(c => c.Name).IsUnique();

        builder.HasQueryFilter(battle => battle.DeletedOnUtc == null);
    }
}
