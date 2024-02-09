using Api.Dtos.Requests;
using Application.Services;

namespace Api.Endpoints;

public static class BattleEndpoint 
{
    public static void MapBattleEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/battle");

        group.MapPost("", async (UpsertDto request, BattleService service) => {
            var result = await service.Create(request.Name);
            return Results.CreatedAtRoute("GetBattleByID", new { result.Id }, result);
        });

        group.MapPut("{id}", async (Guid id,UpsertDto request, BattleService service) => {
            var result = await service.Update(id, request.Name);
            return Results.Ok(result);
        });
        group.MapDelete("", async (Guid id, BattleService service) => {
            await service.Delete(id);
            return Results.NoContent();
        });

        group.MapGet("{id}", async (Guid id, BattleService service) => {
            var result = await service.Get(id);
            return result is null ? Results.NotFound() : Results.Ok(result);
        }).WithName("GetBattleByID");

        group.MapGet("", async (BattleService service) => {
            var result = await service.GetAll();
            return result is null ? Results.Problem() : Results.Ok(result);
        });
    }
}