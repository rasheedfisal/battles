using Api.Dtos.Requests;
using Api.Extensions;
using Application.Services;
using Carter;

namespace Api.Endpoints;

public class HorseEndpoint: ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/horse");

        group.MapPost("", CreateHorse);
        group.MapPut("{id}", UpdateHorse);
        group.MapDelete("", DeleteHorse);
        group.MapGet("{id}", GetHorse).WithName(nameof(GetHorse));
        group.MapGet("", GetAllHorses);
    }
   

    private static async Task<IResult> CreateHorse(UpsertDto request, ILogger<HorseEndpoint> logger, HorseService service, CancellationToken cancellationToken)
    {
        var result = await service.Create(request.Name, cancellationToken);
        if (result.IsFailure)
        {
            logger.LogError("Error accured: {code}: {message}", result.Error.Code, result.Error.Message);
            return result.ToProblemDetails();
        }
        return Results.CreatedAtRoute(nameof(GetHorse), new { result.Value.Id }, result.Value);
    }

    private static async Task<IResult> UpdateHorse(Guid id, UpsertDto request, ILogger<HorseEndpoint> logger, HorseService service, CancellationToken cancellationToken)
    {
        var result = await service.Update(id, request.Name, cancellationToken);
        if (result.IsFailure)
        {
            logger.LogError("Error accured: {code}: {message}", result.Error.Code, result.Error.Message);
            return result.ToProblemDetails();
        }
        return Results.Ok(result.Value);
    }
    private static async Task<IResult> DeleteHorse(Guid id, ILogger<HorseEndpoint> logger, HorseService service, CancellationToken cancellationToken)
    {
        var result = await service.Delete(id, cancellationToken);
        if (result.IsFailure)
        {
            logger.LogError("Error accured: {code}: {message}", result.Error.Code, result.Error.Message);
            return result.ToProblemDetails();
        }
        return Results.NoContent();
    }
    private static async Task<IResult> GetHorse(Guid id, ILogger<HorseEndpoint> logger, HorseService service, CancellationToken cancellationToken)
    {
        var result = await service.Get(id, cancellationToken);
        if (result.IsFailure)
        {
            logger.LogError("Error accured: {code}: {message}", result.Error.Code, result.Error.Message);
            return result.ToNotFoundProblemDetails();
        }
        return Results.Ok(result.Value);
    }

    private static async Task<IResult> GetAllHorses(ILogger<HorseEndpoint> logger, HorseService service, CancellationToken cancellationToken)
    {
        var result = await service.GetAll(cancellationToken);
        if (result.IsFailure)
        {
            logger.LogError("Error accured: {code}: {message}", result.Error.Code, result.Error.Message);
            return result.ToProblemDetails();
        }
        return Results.Ok(result.Value);
    }

    
}