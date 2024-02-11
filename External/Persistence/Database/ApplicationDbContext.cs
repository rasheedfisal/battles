using Application.Core.Common;
using Application.Core.Data;
using Domain.Core.Abstractions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Persistence.Database;

public class ApplicationDbContext: DbContext, IUnitOfWork
{
    private readonly IDateTime _dateTime;
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IDateTime dateTime): base(options)
    {
         _dateTime = dateTime;
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer();
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }

    public DbSet<Battle> Battles {get; set;}
    public DbSet<Samurai> Samurais {get; set;}
    public DbSet<Horse> Horses {get; set;}
    public DbSet<BattleDetail> BattleDetails {get; set;}

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        DateTime utcNow = _dateTime.UtcNow;

        UpdateAuditableEntities(utcNow);

        UpdateSoftDeletableEntities(utcNow);

        return await base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateAuditableEntities(DateTime utcNow)
    {
        foreach (EntityEntry<IAuditableEntity> entityEntry in ChangeTracker.Entries<IAuditableEntity>())
        {
            if (entityEntry.State == EntityState.Added)
            {
                entityEntry.Property(nameof(IAuditableEntity.CreatedOnUtc)).CurrentValue = utcNow;
            }

            if (entityEntry.State == EntityState.Modified)
            {
                entityEntry.Property(nameof(IAuditableEntity.ModifiedOnUtc)).CurrentValue = utcNow;
            }
        }
    }

    private void UpdateSoftDeletableEntities(DateTime utcNow)
    {
        foreach (EntityEntry<ISoftDeletableEntity> entityEntry in ChangeTracker.Entries<ISoftDeletableEntity>())
        {
            if (entityEntry.State != EntityState.Deleted)
            {
                continue;
            }

            entityEntry.Property(nameof(ISoftDeletableEntity.DeletedOnUtc)).CurrentValue = utcNow;

            entityEntry.State = EntityState.Modified;

            UpdateDeletedEntityEntryReferencesToUnchanged(entityEntry);
        }
    }

    private static void UpdateDeletedEntityEntryReferencesToUnchanged(EntityEntry entityEntry)
    {
        if (!entityEntry.References.Any())
        {
            return;
        }

        foreach (ReferenceEntry referenceEntry in entityEntry.References.Where(r => r.TargetEntry!.State == EntityState.Deleted))
        {
            referenceEntry.TargetEntry!.State = EntityState.Unchanged;

            UpdateDeletedEntityEntryReferencesToUnchanged(referenceEntry.TargetEntry);
        }
    }
    
}
