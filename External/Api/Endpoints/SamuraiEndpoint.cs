using Api.Dtos.Requests;
using Api.Extensions;
using Application.Services;
using Domain.Core.Primitives.Result;

namespace Api.Endpoints;

public static class SamuraiEndpoint 
{
    public static void MapSamuraiEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/samurai");

        group.MapPost("", async (UpsertDto request, SamuraiService service, CancellationToken cancellationToken) => {
            var result = await service.Create(request.Name, cancellationToken);
            if (result.IsFailure)
            {
                return result.ToProblemDetails();
            }
            return Results.CreatedAtRoute("GetSamuraiByID", new { result.Value.Id }, result.Value);
        });

        group.MapPut("{id}", async (Guid id,UpsertDto request, SamuraiService service, CancellationToken cancellationToken) => {
            var result = await service.Update(id, request.Name, cancellationToken);
            
            if (result.IsFailure)
            {
                return result.ToProblemDetails();
            }
            
            return Results.Ok(result.Value);
        });
        group.MapDelete("", async (Guid id, SamuraiService service, CancellationToken cancellationToken) => {
            var result = await service.Delete(id, cancellationToken);
            if (result.IsFailure)
            {
                result.ToProblemDetails();
            }
            return Results.NoContent();
        });

        group.MapGet("{id}", async (Guid id, SamuraiService service, CancellationToken cancellationToken) => {
            var result = await service.Get(id, cancellationToken);
            if (result.IsFailure)
            {
                result.ToNotFoundProblemDetails();
            }
            return Results.Ok(result.Value);
        }).WithName("GetSamuraiByID");

        group.MapGet("", async (SamuraiService service, CancellationToken cancellationToken) => {
            var result = await service.GetAll(cancellationToken);
            if (result.IsFailure)
            {
                result.ToProblemDetails();
            }
            return Results.Ok(result.Value);
        });
    }
}