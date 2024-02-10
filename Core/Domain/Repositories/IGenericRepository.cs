using System.Linq.Expressions;

namespace Domain.Repositories;

public interface IGenericRepository<T> where T: class
{
    Task<T?> InsertAsync(T entity);
    Task<T?> UpdateAsync(T entity, Guid Key);
    Task<bool> DeleteAsync(Guid Key);
    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> match, CancellationToken cancellationToken = default);
    Task<T?> FindOneAsync(Expression<Func<T, bool>> match, CancellationToken cancellationToken = default);
    Task<bool> AnyAsync(Expression<Func<T, bool>> match, CancellationToken cancellationToken = default);

}
