using DocumentStoreManagement.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Runtime.CompilerServices;

namespace DocumentStoreManagement.Infrastructure.Repositories.SQL
{
    /// <summary>
    /// Encapsulates all repository transactions.
    /// </summary>
    public class SqlUnitOfWork : IUnitOfWork
    {
        private readonly DbContext _dbContext;
        private readonly IDbConnection _dbConnection;
        private bool _disposed = false;

        // Dictionary to store repositories by type
        private readonly Dictionary<Type, object> _repositories = [];
        private readonly Dictionary<Type, object> _queryRepositories = [];

        /// <summary>
        /// SQL Unit Of Work constructor
        /// </summary>
        /// <param name="dbContext"></param>
        public SqlUnitOfWork(DbContext dbContext)
        {
            _dbContext = dbContext;
            _dbConnection = _dbContext.Database.GetDbConnection();
        }

        /// <inheritdoc/>
        public IRepository<T> Repository<T>() where T : class
        {
            var type = typeof(T);
            if (!_repositories.TryGetValue(type, out object value))
            {
                value = new SqlRepository<T>(_dbContext);
                _repositories[type] = value;
            }
            return (IRepository<T>)value;
        }

        /// <inheritdoc/>
        public IQueryRepository<T> QueryRepository<T>() where T : class
        {
            var type = typeof(T);
            if (!_queryRepositories.TryGetValue(type, out object value))
            {
                value = new SqlQueryRepository<T>(_dbConnection);
                _queryRepositories[type] = value;
            }
            return (IQueryRepository<T>)value;
        }

        /// <inheritdoc/>
        public async Task SaveAsync(CancellationToken cancellationToken = default) => await _dbContext.SaveChangesAsync(cancellationToken);

        /// <inheritdoc/>
        public async Task RefreshMaterializedViewAsync(string viewName)
        {
            await _dbContext.Database.ExecuteSqlAsync(
                FormattableStringFactory.Create(
                    $"REFRESH MATERIALIZED VIEW {viewName}"
                )
            );
        }

        /// <summary>
        /// Cleans up any resources being used.
        /// </summary>
        /// <returns><see cref="ValueTask"/></returns>
        public async ValueTask DisposeAsync()
        {
            await DisposeAsync(true);

            // Take this object off the finalization queue to prevent 
            // finalization code for this object from executing a second time.
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Cleans up any resources being used.
        /// </summary>
        /// <param name="disposing">Whether or not we are disposing</param> 
        /// <returns><see cref="ValueTask"/></returns>
        protected virtual async ValueTask DisposeAsync(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Dispose managed resources.
                    await _dbContext.DisposeAsync();
                }

                // Dispose any unmanaged resources here...
                _disposed = true;
            }
        }
    }
}