namespace GeneralUpdate.Core.Config.Cache
{
    /// <summary>
    /// Cache operation class.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface ICache<TEntity> where TEntity : class
    {
        /// <summary>
        /// add cache.
        /// </summary>
        /// <param name="key">file md5</param>
        /// <param name="entity">configuration file content.</param>
        void TryAdd(string key, TEntity entity);

        /// <summary>
        /// remove cache.
        /// </summary>
        /// <param name="key">file md5</param>
        /// <returns></returns>
        bool TryRemove(string key);

        /// <summary>
        /// get cache configuration file content.
        /// </summary>
        /// <param name="key">file md5</param>
        /// <returns>configuration file content.</returns>
        TEntity TryGet(string key);
    }
}