using System.Linq.Expressions;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace Persistence.Repositories;

public abstract class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected ApplicationDbContext _context;
    internal DbSet<T> dbset;

    public GenericRepository(ApplicationDbContext context)
    {
        _context = context;
        dbset = context.Set<T>();
    }

    public async Task CompleteAsync()
    {
       await _context.SaveChangesAsync();
    }

    public virtual async Task<bool> DeleteAsync(Guid Key)
    {
        // hard delete
        var entity = await dbset.FindAsync(Key);
        if (entity is not null)
        {
            dbset.Remove(entity);
            return true;
        }
        return false;
    }

    public virtual async Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> match)
    {
        return await _context.Set<T>().Where(match).AsNoTracking().ToListAsync();
    }

    public virtual async Task<T?> FindOneAsync(Expression<Func<T, bool>> match)
    {
        return await _context.Set<T>().AsNoTracking().FirstOrDefaultAsync(match);
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
         return await dbset.AsNoTracking().ToListAsync();
    }

    public virtual async Task<T?> InsertAsync(T enity)
    {
        var result = await dbset.AddAsync(enity);
        if (result is null)
        {
            return null;
        }
        return enity;
    }

    public Task<T?> UpdateAsync(T entity, Guid Key)
    {
        throw new NotImplementedException();
    }
}