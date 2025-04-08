using Dapper;
using DocumentStoreManagement.Core.Interfaces;
using DocumentStoreManagement.Core.Models;
using System.Data;

namespace DocumentStoreManagement.Infrastructure.Repositories.SQL
{
    /// <summary>
    /// SQL Query Generic Repository
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SqlQueryRepository<T>(IDbConnection db) : IQueryRepository<T> where T : class
    {
        private readonly IDbConnection _db = db;

        /// <inheritdoc/>
        public async Task<IEnumerable<T>> GetAsync(string query)
        {
            return await _db.QueryAsync<T>(query);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            string query = $"SELECT * FROM {typeof(T).Name}s";
            return await _db.QueryAsync<T>(query);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<T>> GetByDiscriminator(string table)
        {
            string query = $@"SELECT * FROM {table}
                            WHERE ""Discriminator"" = '{typeof(T).Name}'";
            return await _db.QueryAsync<T>(query);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<T>> GetBetweenDatesAsync(string column, string from, string to)
        {
            string query = $@"SELECT * FROM {typeof(T).Name}s
                            WHERE ""{column}""
                            BETWEEN '{from}' AND '{to}'";
            return await _db.QueryAsync<T>(query);
        }

        /// <inheritdoc/>
        public async Task<T> GetByIdAsync(object id)
        {
            string query = $@"SELECT * FROM {typeof(T).Name}s
                            WHERE ""{nameof(BaseEntity.Id)}"" = '{id}'";
            return await _db.QueryFirstOrDefaultAsync<T>(query);
        }
    }
}
