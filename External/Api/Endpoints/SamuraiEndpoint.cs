using Api.Dtos.Requests;
using Application.Services;

namespace Api.Endpoints;

public static class SamuraiEndpoint 
{
    public static void MapSamuraiEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/samurai");

        group.MapPost("", async (UpsertDto request, SamuraiService service) => {
            var result = await service.Create(request.Name);
            return Results.CreatedAtRoute("GetSamuraiByID", new { result.Id }, result);
        });

        group.MapPut("{id}", async (Guid id,UpsertDto request, SamuraiService service) => {
            var result = await service.Update(id, request.Name);
            return Results.Ok(result);
        });
        group.MapDelete("", async (Guid id, SamuraiService service) => {
            await service.Delete(id);
            return Results.NoContent();
        });

        group.MapGet("{id}", async (Guid id, SamuraiService service) => {
            var result = await service.Get(id);
            return result is null ? Results.NotFound() : Results.Ok(result);
        }).WithName("GetSamuraiByID");

        group.MapGet("", async (SamuraiService service) => {
            var result = await service.GetAll();
            return result is null ? Results.Problem() : Results.Ok(result);
        });
    }
}