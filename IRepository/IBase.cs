using NurseryMart.Contract;
using System.Linq.Expressions;

namespace NurseryMart.IRepository
{
    public interface IBase<T>
    {
        Task<long> CountAsync(Expression<Func<T, bool>> filter);
        Task CreateOneAsync(T entity);
        Task CreateManyAsync(IEnumerable<T> entities);
        Task<bool> DeleteOneAsync(Expression<Func<T, bool>> filter);
        Task<bool> UpdateOneAsync(Expression<Func<T, bool>> filter, T entity);
        Task<T> FindOneAsync(Expression<Func<T, bool>> filter);
       
    }
}
