using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;
namespace Api.Extensions;

public static class MigrationManager
{
    public static IEndpointRouteBuilder ApplyMigrations(this IEndpointRouteBuilder app)
    {
        using (var scope = app.ServiceProvider.CreateScope())
        {
            using var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            try
            {
                if (db is not null)
                {
                    SeedData(db, false);
                }
                
            }
            catch (Exception ex)
            {
                //TODO: Log errors using NLog
                Console.WriteLine(new{ex});
                throw;
            }
        }

        return app;
    }

    private static void SeedData(ApplicationDbContext db, bool isProduction) {
        if (isProduction)
        {
            if(db.Database.GetPendingMigrations().Any())
            {
                db.Database.Migrate();
            }
        }else {
            if(!db.Battles.Any()) 
            {
                Console.WriteLine("---> seeding data...");
                db.Battles.AddRange(
                    new Battle(){Id = Guid.NewGuid(), Name = "battle #01", CreatedAt = DateTime.UtcNow},
                    new Battle(){Id = Guid.NewGuid(), Name = "battle #02", CreatedAt = DateTime.UtcNow},
                    new Battle(){Id = Guid.NewGuid(), Name = "battle #03", CreatedAt = DateTime.UtcNow}
                );
            
                db.SaveChanges();
            }
                    
            if(!db.Samurais.Any()) {
                db.Samurais.AddRange(
                    new Samurai(){Id = Guid.NewGuid(), Name = "samurai #01", CreatedAt = DateTime.UtcNow},
                    new Samurai(){Id = Guid.NewGuid(), Name = "samurai #02", CreatedAt = DateTime.UtcNow},
                    new Samurai(){Id = Guid.NewGuid(), Name = "samurai #03", CreatedAt = DateTime.UtcNow}
                );
                db.SaveChanges();
            }

            if(!db.Horses.Any()) {
                db.Horses.AddRange(
                    new Horse(){Id = Guid.NewGuid(), Name = "horse #01", CreatedAt = DateTime.UtcNow},
                    new Horse(){Id = Guid.NewGuid(), Name = "horse #02", CreatedAt = DateTime.UtcNow},
                    new Horse(){Id = Guid.NewGuid(), Name = "horse #03", CreatedAt = DateTime.UtcNow}
                );
                db.SaveChanges();
            }
        }
    }
}
