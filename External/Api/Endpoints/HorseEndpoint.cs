using Api.Dtos.Requests;
using Api.Extensions;
using Application.Services;

namespace Api.Endpoints;

public static class HorseEndpoint 
{
    public static void MapHorseEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/horse");

        group.MapPost("", async (UpsertDto request, HorseService service, CancellationToken cancellationToken) => {
            var result = await service.Create(request.Name, cancellationToken);
            if (result.IsFailure)
            {
                return result.ToProblemDetails();
            }
            return Results.CreatedAtRoute("GetHorseByID", new { result.Value.Id }, result.Value);
        });

        group.MapPut("{id}", async (Guid id,UpsertDto request, HorseService service, CancellationToken cancellationToken) => {
            var result = await service.Update(id, request.Name, cancellationToken);
            if (result.IsFailure)
            {
                return result.ToProblemDetails();
            }
            return Results.Ok(result.Value);
        });
        group.MapDelete("", async (Guid id, HorseService service, CancellationToken cancellationToken) => {
            var result = await service.Delete(id, cancellationToken);
            if (result.IsFailure)
            {
                return result.ToProblemDetails();
            }
            return Results.NoContent();
        });

        group.MapGet("{id}", async (Guid id, HorseService service, CancellationToken cancellationToken) => {
            var result = await service.Get(id, cancellationToken);
            if (result.IsFailure)
            {
                return result.ToNotFoundProblemDetails();
            }
            return Results.Ok(result.Value);
        }).WithName("GetHorseByID");

        group.MapGet("", async (HorseService service, CancellationToken cancellationToken) => {
            var result = await service.GetAll(cancellationToken);
            if (result.IsFailure)
            {
                return result.ToProblemDetails();
            }
            return Results.Ok(result.Value);
        });
    }
}