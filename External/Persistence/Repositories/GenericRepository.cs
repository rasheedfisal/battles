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

    public virtual async Task<bool> AnyAsync(Expression<Func<T, bool>> match, CancellationToken cancellationToken = default)
    {
        return await _context.Set<T>().AnyAsync(match, cancellationToken);
    }

    public async Task CompleteAsync(CancellationToken cancellationToken = default)
    {
       await _context.SaveChangesAsync(cancellationToken);
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

    public virtual async Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> match, CancellationToken cancellationToken = default)
    {
        return await _context.Set<T>().Where(match).AsNoTracking().ToListAsync(cancellationToken);
    }

    public virtual async Task<T?> FindOneAsync(Expression<Func<T, bool>> match, CancellationToken cancellationToken = default)
    {
        return await _context.Set<T>().AsNoTracking().FirstOrDefaultAsync(match, cancellationToken);
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
    {
         return await dbset.AsNoTracking().ToListAsync(cancellationToken);
    }

    public virtual async Task<T?> InsertAsync(T entity)
    {
        var result = await dbset.AddAsync(entity);
        if (result is null)
        {
            return null;
        }
        return entity;
    }

    public virtual async Task<T?> UpdateAsync(T entity, Guid Key)
    {
        if (entity is null)
            return null;

        T? existing = await dbset.FindAsync(Key);

        if (existing is not null)
        {
            _context.Entry(existing).CurrentValues.SetValues(entity);
        }

        return entity;
    }
}