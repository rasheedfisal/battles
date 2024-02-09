using Api.Dtos.Requests;
using Application.Queries;
using Application.Services;

namespace Api.Endpoints;

public static class BattleEndpoint 
{
    public static void MapBattleEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/battle", async (CreateBattleDto request, BattleService battleService) => {
            var battleId = await battleService.CreateAsync(request.Name);
            return Results.Ok(battleId);
        });

        app.MapPut("/api/battle", async (Guid id, BattleService battleService) => {
            await battleService.UpdateAsync(id);
            return Results.NoContent();
        });

        app.MapGet("/api/battle/{id}", async (Guid id, IGetBattleByIdQueryHandler handler) => {
            var battle = await handler.Handle(id);
            return battle is null ? Results.NotFound() : Results.Ok(battle);
        });
    }
}