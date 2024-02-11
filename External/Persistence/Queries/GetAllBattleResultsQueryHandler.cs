using Application.Queries;
using Domain.Core.Primitives.Result;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace Persistence.Queries;

public class GetAllBattleResultsQueryHandler : IGetAllBattleResultsQueryHandler
{
    private readonly ApplicationDbContext _context;

    public GetAllBattleResultsQueryHandler(ApplicationDbContext Context)
    {
        _context = Context;
    }

    public async Task<Result<List<BattleResultResponse>>> Handle(CancellationToken cancellationToken)
    {
        var battleResults = new List<BattleResultResponse>();

        var battles = await _context
            .Battles
            .AsNoTracking()
            .ToListAsync(cancellationToken);
        
        foreach (var battle in battles)
        {
            var samuraiHorses = await _context
                .BattleDetails
                .AsNoTracking()
                .Include(x => x.Samurai)
                .Include(x => x.Horse)
                .Where(x => x.BattleId == battle.Id)
                .ToListAsync(cancellationToken);
            if (samuraiHorses.Count > 0)
            {
                var newResult = new BattleResultResponse{
                        battle = battle,
                        SamuraiHorses = samuraiHorses.Select(x =>
                            new SamuraiHorseResponse
                            {
                                Horse = x.Horse!,
                                Samurai = x.Samurai!
                            }
                        ).ToList()
                    };
                battleResults.Add(newResult);
            }
        
        }

       return Result.Success(battleResults);
    }
}
