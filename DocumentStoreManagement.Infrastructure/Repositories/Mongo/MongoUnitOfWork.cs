using DocumentStoreManagement.Core.Interfaces;

namespace DocumentStoreManagement.Infrastructure.Repositories.Mongo
{
    /// <summary>
    /// Encapsulates all repository transactions
    /// </summary>
    public class MongoUnitOfWork : IUnitOfWork
    {
        /// <inheritdoc/>
        public async Task SaveAsync(CancellationToken cancellationToken = default) => await Task.CompletedTask;

        /// <inheritdoc />
        public IRepository<T> Repository<T>() where T : class
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IQueryRepository<T> QueryRepository<T>() where T : class
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public async Task RefreshMaterializedViewAsync(string viewName) => await Task.CompletedTask;

        /// <summary>
        /// Cleans up any resources being used.
        /// </summary>
        public async ValueTask DisposeAsync()
        {
            // Take this object off the finalization queue to prevent 
            // finalization code for this object from executing a second time.
            GC.SuppressFinalize(this);
            await Task.CompletedTask;
        }
    }
}
