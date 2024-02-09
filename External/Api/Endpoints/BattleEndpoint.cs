using Api.Dtos.Requests;
using Application.Queries;
using Application.Services;

namespace Api.Endpoints;

public static class BattleEndpoint 
{
    public static void MapBattleEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/battle");

        group.MapPost("", async (CreateBattleDto request, BattleService battleService) => {
            var battleId = await battleService.Create(request.Name);
            return Results.Ok(battleId);
        });

        group.MapPut("", async (Guid id, BattleService battleService) => {
            await battleService.Update(id);
            return Results.NoContent();
        });
        group.MapDelete("", async (Guid id, BattleService battleService) => {
            await battleService.Delete(id);
            return Results.NoContent();
        });

        group.MapGet("{id}", async (Guid id, BattleService battleService) => {
            var battle = await battleService.Get(id);
            return battle is null ? Results.NotFound() : Results.Ok(battle);
        });
        group.MapGet("", async (BattleService battleService) => {
            var battles = await battleService.GetAll();
            return battles is null ? Results.Problem() : Results.Ok(battles);
        });
    }
}