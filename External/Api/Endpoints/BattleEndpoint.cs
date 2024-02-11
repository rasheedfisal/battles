using Api.Dtos.Requests;
using Api.Extensions;
using Application.Commands;
using Application.Queries;
using Application.Services;
using Carter;

namespace Api.Endpoints;

public class BattleEndpoint: ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/battle");

        group.MapPost("", CreateBattle);
        group.MapPut("{id}", UpdateBattle);
        group.MapDelete("", DeleteBattle);
        group.MapGet("{id}", GetBattle).WithName(nameof(GetBattle));
        group.MapGet("", GetAllBattles);
        group.MapPost("start/{battleId}", StartBattle);
        group.MapPatch("end/{battleId}", EndBattle);
        group.MapGet("results", GetBattleResults);
    }

    private static async Task<IResult> CreateBattle(UpsertDto request, ILogger<BattleEndpoint> logger, BattleService service, CancellationToken cancellationToken)
    {
        var result = await service.Create(request.Name, cancellationToken);
            if (result.IsFailure)
            {
                logger.LogError("Error accured: {code}: {message}", result.Error.Code, result.Error.Message);
                return result.ToProblemDetails();
            }
            return Results.CreatedAtRoute(nameof(GetBattle), new { result.Value.Id }, result.Value);
    }

    private static async Task<IResult> UpdateBattle(Guid id,UpsertDto request, ILogger<BattleEndpoint> logger, BattleService service, CancellationToken cancellationToken)
    {
        var result = await service.Update(id, request.Name, cancellationToken);
        if (result.IsFailure)
        {
            logger.LogError("Error accured: {code}: {message}", result.Error.Code, result.Error.Message);
            return result.ToProblemDetails();
        }
        return Results.Ok(result.Value);
    }
    private static async Task<IResult> DeleteBattle(Guid id, BattleService service, ILogger<BattleEndpoint> logger, CancellationToken cancellationToken)
    {
        var result = await service.Delete(id, cancellationToken);
        if (result.IsFailure)
        {
            logger.LogError("Error accured: {code}: {message}", result.Error.Code, result.Error.Message);
            return result.ToProblemDetails();
        }
        return Results.NoContent();
    }
    private static async Task<IResult> GetBattle(Guid id, BattleService service, ILogger<BattleEndpoint> logger, CancellationToken cancellationToken)
    {
         var result = await service.Get(id, cancellationToken);
        if (result.IsFailure)
        {
            logger.LogError("Error accured: {code}: {message}", result.Error.Code, result.Error.Message);
            return result.ToNotFoundProblemDetails();
        }
        return Results.Ok(result.Value);
    }
    private static async Task<IResult> GetAllBattles(BattleService service, ILogger<BattleEndpoint> logger, CancellationToken cancellationToken)
    {
        var result = await service.GetAll(cancellationToken);
        if (result.IsFailure)
        {
            logger.LogError("Error accured: {code}: {message}", result.Error.Code, result.Error.Message);
            return result.ToProblemDetails();
        }
        return Results.Ok(result.Value);
    }

    private static async Task<IResult> StartBattle(Guid battleId, ILogger<BattleEndpoint> logger, IStartBattleCommandHandler handler, CancellationToken cancellationToken)
    {
        var result = await handler.Handle(battleId, cancellationToken);
            if (result.IsFailure)
            {
                logger.LogError("Error accured: {code}: {message}", result.Error.Code, result.Error.Message);
                return result.ToProblemDetails();
            }
            return Results.Ok();
    }
    private static async Task<IResult> EndBattle(Guid battleId, ILogger<BattleEndpoint> logger, IEndBattleCommandHandler handler, CancellationToken cancellationToken)
    {
        var result = await handler.Handle(battleId, cancellationToken);
            if (result.IsFailure)
            {
                logger.LogError("Error accured: {code}: {message}", result.Error.Code, result.Error.Message);
                return result.ToProblemDetails();
            }
            return Results.Ok();
    }
    private static async Task<IResult> GetBattleResults(ILogger<BattleEndpoint> logger, IGetAllBattleResultsQueryHandler handler, CancellationToken cancellationToken)
    {
        var result = await handler.Handle(cancellationToken);
            if (result.IsFailure)
            {
                logger.LogError("Error accured: {code}: {message}", result.Error.Code, result.Error.Message);
                return result.ToProblemDetails();
            }
            return Results.Ok(result.Value);
    }

    
}