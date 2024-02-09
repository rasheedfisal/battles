using Application.Queries;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace Persistence.Queries;

public class GetBattleByIdQueryHandler : IGetBattleByIdQueryHandler
{
    private readonly ApplicationDbContext _dbContext;

    public GetBattleByIdQueryHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<BattleResponse?> Handle(Guid id)
    {
        var battleResponse = await _dbContext
            .Battles
            .AsNoTracking()
            .Where(battle => battle.Id == id)
            .Select(battle => new BattleResponse{
                Id = battle.Id,
                Name = battle.Name,
                BattleStartDate = battle.BattleStartDate,
                BattleEndDate = battle.BattleEndDate
            })
            .FirstOrDefaultAsync();

            return battleResponse;
    }
}
