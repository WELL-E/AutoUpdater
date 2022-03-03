namespace GeneralUpdate.Core.Config.Handles
{
    public interface IHandle<TEntity> where TEntity : class
    {
        /// <summary>
        /// Write the cache content to the file to be updated.
        /// </summary>
        /// <param name="path">file path</param>
        /// <param name="entities">file content</param>
        /// <returns></returns>
        bool Write(string path, TEntity entities);

        /// <summary>
        /// read file content.
        /// </summary>
        /// <param name="path">file path</param>
        /// <returns></returns>
        TEntity Read(string path);
    }
}