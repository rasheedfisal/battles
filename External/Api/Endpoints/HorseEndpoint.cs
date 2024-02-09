using Api.Dtos.Requests;
using Application.Services;

namespace Api.Endpoints;

public static class HorseEndpoint 
{
    public static void MapHorseEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/horse");

        group.MapPost("", async (UpsertDto request, HorseService service) => {
            var result = await service.Create(request.Name);
            return Results.CreatedAtRoute("GetHorseByID", new { result.Id }, result);
        });

        group.MapPut("{id}", async (Guid id,UpsertDto request, HorseService service) => {
            var result = await service.Update(id, request.Name);
            return Results.Ok(result);
        });
        group.MapDelete("", async (Guid id, HorseService service) => {
            await service.Delete(id);
            return Results.NoContent();
        });

        group.MapGet("{id}", async (Guid id, HorseService service) => {
            var result = await service.Get(id);
            return result is null ? Results.NotFound() : Results.Ok(result);
        }).WithName("GetHorseByID");

        group.MapGet("", async (HorseService service) => {
            var result = await service.GetAll();
            return result is null ? Results.Problem() : Results.Ok(result);
        });
    }
}