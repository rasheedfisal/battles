using System.Linq.Expressions;

namespace Domain.Repositories;

public interface IGenericRepository<T> where T: class
{
    Task<T?> InsertAsync(T enity);
    Task<T?> UpdateAsync(T entity, Guid Key);
    Task<bool> DeleteAsync(Guid Key);
    Task<IEnumerable<T>> GetAllAsync();
    Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> match);
    Task<T?> FindOneAsync(Expression<Func<T, bool>> match);
    Task CompleteAsync();

}
