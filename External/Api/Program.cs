using Api.Configurations;
using Api.Endpoints;
using Api.Extensions;
using Api.Infrastructure;
using Carter;
using NLog;
using NLog.Web;


var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("init main");

try
{

    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services
        .AddDB(builder)
        .AddServices()
        .AddPersistence();

    builder.Services.AddExceptionHandler<GlobalErrorHandler>();
    builder.Services.AddProblemDetails();

    builder.Services.AddCarter();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        app.ApplyMigrations();
    }

    app.UseHttpsRedirection();

    app.UseExceptionHandler();

    app.MapCarter();

    app.Run();

}
catch(Exception ex)
{
    logger.Error(ex);
}
finally
{
    LogManager.Shutdown();
}
