using System.Data;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using NurseryMart.IRepository;
using NurseryMart.Utility;

namespace NurseryMart.Repositories
{
    public class RepositoryBase<T> where T : IEntity
    {
        private readonly SqlConnectionFactory _connectionFactory;

        // Constructor for dependency injection (passing the connection factory)
        public RepositoryBase(SqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        // Count: Get count of records matching a filter.
        public async Task<long> CountAsync(Expression<Func<T, bool>> filter)
        {
            using (var connection = _connectionFactory.CreateConnection())
            {
                var query = "SELECT COUNT(*) FROM " + typeof(T).Name.ToSlugCase() + " WHERE " + GetSqlWhereClause(filter);

                await connection.OpenAsync();
                using (var command = new SqlCommand(query, connection))
                {
                    return (long)await command.ExecuteScalarAsync(); // Returns the count.
                }
            }
        }

        // CreateOne: Insert a single entity into the database.
        public async Task CreateOneAsync(T entity)
        {
            using (var connection = _connectionFactory.CreateConnection())
            {
                var query = "INSERT INTO " + typeof(T).Name.ToSlugCase() + " (columns...) VALUES (@values...)";
                await connection.OpenAsync();

                using (var command = new SqlCommand(query, connection))
                {
                    AddParameters(command, entity); // Method to map entity properties to SQL parameters.
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        // CreateMany: Insert multiple entities.
        public async Task CreateManyAsync(IEnumerable<T> entities)
        {
            using (var connection = _connectionFactory.CreateConnection())
            {
                var query = "INSERT INTO " + typeof(T).Name.ToSlugCase() + " (columns...) VALUES (@values...)";
                await connection.OpenAsync();

                using (var command = new SqlCommand(query, connection))
                {
                    foreach (var entity in entities)
                    {
                        AddParameters(command, entity);
                        await command.ExecuteNonQueryAsync(); // Insert each entity
                    }
                }
            }
        }

        // DeleteOne: Delete a single record by a filter.
        public async Task<bool> DeleteOneAsync(Expression<Func<T, bool>> filter)
        {
            using (var connection = _connectionFactory.CreateConnection())
            {
                var query = "DELETE FROM " + typeof(T).Name.ToSlugCase() + " WHERE " + GetSqlWhereClause(filter);
                await connection.OpenAsync();

                using (var command = new SqlCommand(query, connection))
                {
                    return await command.ExecuteNonQueryAsync() > 0; // Returns true if any row was deleted.
                }
            }
        }

        // UpdateOne: Update a single entity by a filter.
        public async Task<bool> UpdateOneAsync(Expression<Func<T, bool>> filter, T entity)
        {
            using (var connection = _connectionFactory.CreateConnection())
            {
                var query = "UPDATE " + typeof(T).Name.ToSlugCase() + " SET columns... WHERE " + GetSqlWhereClause(filter);
                await connection.OpenAsync();

                using (var command = new SqlCommand(query, connection))
                {
                    AddParameters(command, entity);
                    return await command.ExecuteNonQueryAsync() > 0; // Returns true if any row was updated.
                }
            }
        }

        // FindOne: Find a single record by a filter.
        public async Task<T> FindOneAsync(Expression<Func<T, bool>> filter)
        {
            using (var connection = _connectionFactory.CreateConnection())
            {
                var query = "SELECT * FROM " + typeof(T).Name.ToSlugCase() + " WHERE " + GetSqlWhereClause(filter);
                await connection.OpenAsync();

                using (var command = new SqlCommand(query, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return MapReaderToEntity(reader); // Map the reader data to the entity object.
                        }
                    }
                }

                return default(T); // Return default if no record is found.
            }
        }

        // GetSqlWhereClause: Convert a LINQ Expression (filter) to an SQL WHERE clause.
        private string GetSqlWhereClause(Expression<Func<T, bool>> filter)
        {
            // We use expression trees to parse the expression and convert it to SQL format.
            var body = filter.Body as BinaryExpression;

            if (body == null)
                throw new ArgumentException("Filter expression should be a binary expression.");

            var left = body.Left as MemberExpression;
            var right = body.Right as ConstantExpression;

            if (left == null || right == null)
                throw new ArgumentException("Only equality expressions are supported for this method.");

            // Here, we assume a simple equality check (e.g., `a.Property == value`)
            var columnName = left.Member.Name;
            var columnValue = right.Value;

            // Return SQL WHERE clause (simple equality).
            return $"{columnName} = @{columnName}";
        }

        // AddParameters: Add parameters to the SqlCommand object based on the entity.
        private void AddParameters(SqlCommand command, T entity)
        {
            // Get all public properties of the entity type (T)
            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                var value = property.GetValue(entity);

                // Avoid adding null values
                if (value != null)
                {
                    command.Parameters.Add(new SqlParameter($"@{property.Name}", value ?? DBNull.Value));
                }
            }
        }

        // MapReaderToEntity: Convert a SqlDataReader row to an instance of the entity type.
        private T MapReaderToEntity(SqlDataReader reader)
        {
            // Create a new instance of the entity (T)
            var entity = Activator.CreateInstance<T>();

            // Loop through all properties of the entity
            foreach (var property in typeof(T).GetProperties())
            {
                // Make sure the property can be set (it must have a setter)
                if (property.CanWrite)
                {
                    // Get the value from the reader based on the column name
                    var value = reader[property.Name];

                    // If the value is not DBNull, set it to the entity property
                    if (value != DBNull.Value)
                    {
                        property.SetValue(entity, value);
                    }
                }
            }

            return entity;
        }

        // Example method to find one entity by a filter.
        //public async Task<T> FindOneAsync(Expression<Func<T, bool>> filter)
        //{
        //    using (var connection = _connectionFactory.CreateConnection())
        //    {
        //        // Convert filter expression to SQL WHERE clause
        //        var whereClause = GetSqlWhereClause(filter);
        //        var query = $"SELECT * FROM {typeof(T).Name} WHERE {whereClause}";

        //        await connection.OpenAsync();

        //        using (var command = new SqlCommand(query, connection))
        //        {
        //            // Add parameters to the command based on the entity's property values
        //            AddParameters(command, new T()); // Or pass an actual entity instance

        //            using (var reader = await command.ExecuteReaderAsync())
        //            {
        //                if (await reader.ReadAsync())
        //                {
        //                    return MapReaderToEntity(reader);
        //                }
        //            }
        //        }
        //    }

        //    return default(T); // Return default if no result found
        //}
    }
}
