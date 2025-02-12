using System.Data;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NurseryMart.Contract;
using NurseryMart.IRepository;
using NurseryMart.Utility;

namespace NurseryMart.Repositories
{
    public class RepositoryBase<T> : IBase<T> where T : class
    {
        private readonly DbContext _context;
        private readonly DbSet<T> _dbSet;

        public RepositoryBase(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<T> FindOneAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken)
        {
            return await _dbSet.FirstOrDefaultAsync(filter, cancellationToken);
        }

        public async Task<IEnumerable<T>> FindManyAsync(Expression<Func<T, bool>> filter, Pagination pagination, CancellationToken cancellationToken)
        {
            IQueryable<T> query = _dbSet.AsQueryable();
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (!pagination.SkipPagination)
            {
                query = query.Skip((pagination.PageNumber - 1) * pagination.PageSize)
                             .Take(pagination.PageSize);
            }
            // Apply sorting if necessary (you can extend this further for more complex sorting logic)
            if (!string.IsNullOrEmpty(pagination.SortColumn))
            {
                if (pagination.SortDesc)
                {
                    query = query.OrderByDescending(x => EF.Property<object>(x, pagination.SortColumn));
                }
                else
                {
                    query = query.OrderBy(x => EF.Property<object>(x, pagination.SortColumn));
                }
            }

            return await query.ToListAsync(cancellationToken);
        }

        // UpdateOneAsync - Update a single entity
        public async Task<bool> UpdateOneAsync(T entity, CancellationToken cancellationToken)
        {
            _dbSet.Update(entity);
            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }

        // UpdateManyAsync - Update multiple entities
        public async Task<bool> UpdateManyAsync(IEnumerable<T> entities, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync(cancellationToken))
            {
                try
                {
                    _dbSet.UpdateRange(entities);
                    var result = await _context.SaveChangesAsync(cancellationToken) > 0;
                    if (result)
                    {
                        await transaction.CommitAsync(cancellationToken);
                    }
                    return result;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    throw new Exception("An error occurred while updating entities.", ex);
                }
            }
        }

        // CountAsync - Get the count of entities that match a given filter
        public async Task<int> CountAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken)
        {
            if (filter != null)
            {
                return await _dbSet.CountAsync(filter, cancellationToken);
            }
            return await _dbSet.CountAsync(cancellationToken);
        }

        // CreateOneAsync - Create a single entity
        public async Task CreateOneAsync(T entity, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync(cancellationToken))
            {
                try
                {
                    await _dbSet.AddAsync(entity, cancellationToken);
                    await _context.SaveChangesAsync(cancellationToken);
                    await transaction.CommitAsync(cancellationToken);  // Commit the transaction
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync(cancellationToken);  // Rollback on failure
                    throw;
                }
            }
        }

        // CreateManyAsync - Create multiple entities
        public async Task<IEnumerable<T>> CreateManyAsync(IEnumerable<T> entities, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync(cancellationToken))
            {
                try
                {
                    await _dbSet.AddRangeAsync(entities, cancellationToken);
                    await _context.SaveChangesAsync(cancellationToken);
                    await transaction.CommitAsync(cancellationToken);
                    return entities;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    throw new Exception("An error occurred while creating entities.", ex);
                }
            }
        }


    }
}
