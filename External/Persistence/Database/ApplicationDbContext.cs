using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Database;

public class ApplicationDbContext: DbContext
{
    public ApplicationDbContext(DbContextOptions options): base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }

    public DbSet<Battle> Battles {get; set;}
}
