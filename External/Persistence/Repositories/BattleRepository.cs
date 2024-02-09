using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace Persistence.Repositories;

public class BattleRepository : IBattleRepository
{
    private readonly ApplicationDbContext _dbContext;
    public BattleRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Battle?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Battles.FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<Guid> InsertAsync(Battle battle)
    {
        _dbContext.Battles.Add(battle);

        await _dbContext.SaveChangesAsync();

        return battle.Id;
    }

    public async Task UpdateAsync(Battle battle)
    {
        _dbContext.Update(battle);

        await _dbContext.SaveChangesAsync();
    }
}
