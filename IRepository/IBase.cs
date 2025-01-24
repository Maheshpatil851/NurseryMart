using NurseryMart.Contract;
using System.Linq.Expressions;

namespace NurseryMart.IRepository
{
    public interface IBase<T>
    {
        Task<T> FindOneAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken);
        Task<IEnumerable<T>> FindManyAsync( Expression<Func<T, bool>> filter, Pagination pagination, CancellationToken cancellationToken);
        Task<bool> UpdateOneAsync(T entity, CancellationToken cancellationToken);
        Task<bool> UpdateManyAsync(IEnumerable<T> entities, CancellationToken cancellationToken);
        Task<int> CountAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken);
        Task<T> CreateOneAsync(T entity, CancellationToken cancellationToken);
        Task<IEnumerable<T>> CreateManyAsync(IEnumerable<T> entities, CancellationToken cancellationToken);
    }
}
