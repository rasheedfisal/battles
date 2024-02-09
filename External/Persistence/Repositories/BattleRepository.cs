using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace Persistence.Repositories;

public class BattleRepository : GenericRepository<Battle>, IBattleRepository
{
    public BattleRepository(ApplicationDbContext context) : base(context)
    {
    }
}
