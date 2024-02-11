using Api.Dtos.Requests;
using Api.Extensions;
using Application.Services;
using Carter;
using Domain.Core.Primitives.Result;

namespace Api.Endpoints;

public class SamuraiEndpoint: ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/samurai");

        group.MapPost("", CreateSamurai);
        group.MapPut("{id}", UpdateSamurai);
        group.MapDelete("", DeleteSamurai);
        group.MapGet("{id}", GetSamurai).WithName("GetSamuraiByID");
        group.MapGet("", GetAllSamurais);
    }
   

     private static async Task<IResult> CreateSamurai(UpsertDto request, ILogger<SamuraiEndpoint> logger, SamuraiService service, CancellationToken cancellationToken)
     {
        var result = await service.Create(request.Name, cancellationToken);
        if (result.IsFailure)
        {
            logger.LogError("Error accured: {code}: {message}", result.Error.Code, result.Error.Message);
            return result.ToProblemDetails();
        }
        return Results.CreatedAtRoute("GetSamuraiByID", new { result.Value.Id }, result.Value);
     }
     private static async Task<IResult> UpdateSamurai(Guid id, UpsertDto request, ILogger<SamuraiEndpoint> logger, SamuraiService service, CancellationToken cancellationToken)
     {
        var result = await service.Update(id, request.Name, cancellationToken);
        if (result.IsFailure)
        {
            logger.LogError("Error accured: {code}: {message}", result.Error.Code, result.Error.Message);
            return result.ToProblemDetails();
        }
        
        return Results.Ok(result.Value);
     }
     private static async Task<IResult> DeleteSamurai(Guid id, ILogger<SamuraiEndpoint> logger, SamuraiService service, CancellationToken cancellationToken)
     {
        var result = await service.Delete(id, cancellationToken);
        if (result.IsFailure)
        {
            logger.LogError("Error accured: {code}: {message}", result.Error.Code, result.Error.Message);
            result.ToProblemDetails();
        }
        return Results.NoContent();
     }
     private static async Task<IResult> GetSamurai(Guid id, ILogger<SamuraiEndpoint> logger, SamuraiService service, CancellationToken cancellationToken)
     {
        var result = await service.Get(id, cancellationToken);
        if (result.IsFailure)
        {
            logger.LogError("Error accured: {code}: {message}", result.Error.Code, result.Error.Message);
            result.ToNotFoundProblemDetails();
        }
        return Results.Ok(result.Value);
     }
     private static async Task<IResult> GetAllSamurais(ILogger<SamuraiEndpoint> logger, SamuraiService service, CancellationToken cancellationToken)
     {
        var result = await service.GetAll(cancellationToken);
        if (result.IsFailure)
        {
            logger.LogError("Error accured: {code}: {message}", result.Error.Code, result.Error.Message);
            result.ToProblemDetails();
        }
        return Results.Ok(result.Value);
     }

    
}