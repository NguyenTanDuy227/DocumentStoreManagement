namespace DocumentStoreManagement.Core.Interfaces
{
    /// <summary>
    /// Generic Unit Of Work interface
    /// </summary>
    public interface IUnitOfWork : IAsyncDisposable
    {
        /// <summary>
        /// Gets the repository for entity type T.
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <returns>Repository instance</returns>
        IRepository<T> Repository<T>() where T : class;

        /// <summary>
        /// Gets the query repository for entity type T.
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <returns>Query repository instance</returns>
        IQueryRepository<T> QueryRepository<T>() where T : class;

        /// <summary>
        /// Save changes
        /// </summary>
        /// <returns>Nothing</returns>
        Task SaveAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Refresh Materialized View
        /// </summary>
        /// <param name="viewName"></param>
        /// <returns>Nothing</returns>
        Task RefreshMaterializedViewAsync(string viewName);
    }
}
