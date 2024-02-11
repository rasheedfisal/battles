using Application.Commands;
using Application.Core.Common;
using Application.Core.Data;
using Application.Queries;
using Application.Services;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence.Commands;
using Persistence.Common;
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
        services.AddScoped<IGetAllBattleResultsQueryHandler, GetAllBattleResultsQueryHandler>();
        services.AddScoped<IStartBattleCommandHandler, StartBattleCommandHandler>();
        services.AddScoped<IEndBattleCommandHandler, EndBattleCommandHandler>();
        services.AddTransient<IDateTime, MachineDateTime>();
        return services;
    }
    public static IServiceCollection AddDB(this IServiceCollection services, WebApplicationBuilder builder)
    {
        
        services.AddDbContext<ApplicationDbContext>(opt => {
            var connectionString = builder.Configuration.GetConnectionString("DB_Conn");
                opt.UseSqlServer(connectionString);
        });

        services.AddScoped<IUnitOfWork>(serviceProvider => serviceProvider.GetRequiredService<ApplicationDbContext>());
        // services.AddDbContext<ApplicationDbContext>(opt => opt.UseInMemoryDatabase("InMem"));
        return services;
    }
    
}
