using Api.Dtos.Requests;
using Api.Extensions;
using Application.Services;

namespace Api.Endpoints;

public static class BattleEndpoint 
{
    public static void MapBattleEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/battle");

        group.MapPost("", async (UpsertDto request, BattleService service, CancellationToken cancellationToken) => {
            var result = await service.Create(request.Name, cancellationToken);
            if (result.IsFailure)
            {
                return result.ToProblemDetails();
            }
            return Results.CreatedAtRoute("GetBattleByID", new { result.Value.Id }, result.Value);
        });

        group.MapPut("{id}", async (Guid id,UpsertDto request, BattleService service, CancellationToken cancellationToken) => {
            var result = await service.Update(id, request.Name, cancellationToken);
            if (result.IsFailure)
            {
                return result.ToProblemDetails();
            }
            return Results.Ok(result.Value);
        });
        group.MapDelete("", async (Guid id, BattleService service, CancellationToken cancellationToken) => {
            var result = await service.Delete(id, cancellationToken);
            if (result.IsFailure)
            {
                return result.ToProblemDetails();
            }
            return Results.NoContent();
        });

        group.MapGet("{id}", async (Guid id, BattleService service, CancellationToken cancellationToken) => {
            var result = await service.Get(id, cancellationToken);
            if (result.IsFailure)
            {
                return result.ToNotFoundProblemDetails();
            }
            return Results.Ok(result.Value);
        }).WithName("GetBattleByID");

        group.MapGet("", async (BattleService service, CancellationToken cancellationToken) => {
            var result = await service.GetAll(cancellationToken);
            if (result.IsFailure)
            {
                return result.ToProblemDetails();
            }
            return Results.Ok(result.Value);
        });
    }
}