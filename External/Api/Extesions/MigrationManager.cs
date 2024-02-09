using Domain.Entities;
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
                    if(!db.Battles.Any()) 
                    {
                        Console.WriteLine("---> seeding data...");
                        db.Battles.AddRange(
                            new Battle(){Id = Guid.NewGuid(), Name = "battle #01", BattleStartDate = DateTime.UtcNow, BattleEndDate = DateTime.UtcNow},
                            new Battle(){Id = Guid.NewGuid(), Name = "battle #02", BattleStartDate = DateTime.UtcNow, BattleEndDate = DateTime.UtcNow},
                            new Battle(){Id = Guid.NewGuid(), Name = "battle #03", BattleStartDate = DateTime.UtcNow, BattleEndDate = DateTime.UtcNow}
                        );
                    
                        db.SaveChanges();
                    }
                    // if(db.Database.GetPendingMigrations().Any())
                    // {
                    //     db.Database.Migrate();
                    // }
                }
                
            }
            catch (Exception ex)
            {
                //TODO: Log errors
                throw;
            }
        }

        return app;
    }
}
