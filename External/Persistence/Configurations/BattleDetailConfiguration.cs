
using Domain.Core.ValueObjects;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

internal class BattleDetailConfiguration : IEntityTypeConfiguration<BattleDetail>
{
    public void Configure(EntityTypeBuilder<BattleDetail> builder)
    {
        builder.HasKey(c => c.Id);

        builder.HasOne<Battle>()
            .WithMany()
            .HasForeignKey(b => b.BattleId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<Samurai>()
            .WithMany()
            .HasForeignKey(b => b.SamuraiId)
            .IsRequired();

        builder.HasOne<Horse>()
            .WithMany()
            .HasForeignKey(b => b.HorseId)
            .IsRequired();

        builder.Property(c => c.HorseRideStartDate)
            .IsRequired();

        builder.Property(c => c.HorseRideEndDate);

        builder.HasIndex(p => new{ p.BattleId, p.SamuraiId, p.HorseId });
    }
}
