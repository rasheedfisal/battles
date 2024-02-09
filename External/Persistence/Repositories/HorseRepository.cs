using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace Persistence.Repositories;

public class HorseRepository : GenericRepository<Horse>, IHorseRepository
{
    public HorseRepository(ApplicationDbContext context) : base(context)
    {
    }
}
