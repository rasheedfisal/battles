using Application.Queries;
using Application.Services;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;
using Persistence.Queries;
using Persistence.Repositories;

namespace Api.Configurations;

public static class DependancyInjection
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<BattleService>();
        services.AddScoped<SamuraiService>();
        services.AddScoped<HorseService>();
        return services;
    }
    public static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        services.AddScoped<IBattleRepository, BattleRepository>();
        services.AddScoped<ISamuraiRepository, SamuraiRepository>();
        services.AddScoped<IHorseRepository, HorseRepository>();
        services.AddScoped<IGetBattleByIdQueryHandler, GetBattleByIdQueryHandler>();
        return services;
    }
    public static IServiceCollection AddDB(this IServiceCollection services, WebApplicationBuilder builder)
    {
        
        services.AddDbContext<ApplicationDbContext>(opt => {
            var connectionString = builder.Configuration.GetConnectionString("DB_Conn");
                opt.UseSqlServer(connectionString);
        });
        // services.AddDbContext<ApplicationDbContext>(opt => opt.UseInMemoryDatabase("InMem"));
        return services;
    }
    
}
