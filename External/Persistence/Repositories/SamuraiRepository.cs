using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace Persistence.Repositories;

public class SamuraiRepository : GenericRepository<Samurai>, ISamuraiRepository
{
    public SamuraiRepository(ApplicationDbContext context) : base(context)
    {
    }
}
