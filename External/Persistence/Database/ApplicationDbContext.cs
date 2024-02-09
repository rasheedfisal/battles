using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Database;

public class ApplicationDbContext: DbContext
{
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
    {

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
    
}
