namespace DocumentStoreManagement.Core.Interfaces
{
    /// <summary>
    /// Generic Query Repository interface
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IQueryRepository<T> where T : class
    {
        /// <summary>
        /// Gets by query and returns as DTO
        /// </summary>
        /// <param name="query"></param>
        /// <returns>List of data</returns>
        Task<IEnumerable<T>> GetAsync(string query);

        /// <summary>
        /// Gets all the data from a table
        /// </summary>
        /// <returns>List of data</returns>
        Task<IEnumerable<T>> GetAllAsync();

        /// <summary>
        /// Get all the data from a table by a discriminator
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        Task<IEnumerable<T>> GetByDiscriminator(string table);

        /// <summary>
        /// Gets all the data from a table which between 2 dates
        /// </summary>
        /// <param name="column"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns>A list of data</returns>
        Task<IEnumerable<T>> GetBetweenDatesAsync(string column, string from, string to);

        /// <summary>
        /// Gets an item by its Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>An item which matches the Id</returns>
        Task<T> GetByIdAsync(object id);
    }
}
