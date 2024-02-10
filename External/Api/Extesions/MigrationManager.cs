using Domain.Core.ValueObjects;
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
            // if(!db.Battles.Any()) 
            // {
            //     Console.WriteLine("---> seeding data...");
            //     db.Battles.AddRange(
            //         Battle.Create(Name.Create("battle #01").Value, DateTime.UtcNow),
            //         Battle.Create(Name.Create("battle #02").Value, DateTime.UtcNow),
            //         Battle.Create(Name.Create("battle #03").Value, DateTime.UtcNow)
            //     );
            
            //     db.SaveChanges();
            // }
                    
            // if(!db.Samurais.Any()) {
            //     db.Samurais.AddRange(
            //         Samurai.Create(Name.Create("samurai #01").Value, DateTime.UtcNow),
            //         Samurai.Create(Name.Create("samurai #02").Value, DateTime.UtcNow),
            //         Samurai.Create(Name.Create("samurai #03").Value, DateTime.UtcNow)
            //     );
            //     db.SaveChanges();
            // }

            // if(!db.Horses.Any()) {
            //     db.Horses.AddRange(
            //         Horse.Create(Name.Create("horse #01").Value, DateTime.UtcNow),
            //         Horse.Create(Name.Create("horse #02").Value, DateTime.UtcNow),
            //         Horse.Create(Name.Create("horse #03").Value, DateTime.UtcNow)
            //     );
            //     db.SaveChanges();
            // }
        }
    }
}
