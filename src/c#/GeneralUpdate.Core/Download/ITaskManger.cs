namespace GeneralUpdate.Core.Download
{
    /// <summary>
    /// Download task interface.
    /// </summary>
    /// <typeparam name="T">'T' is the version information that needs to be downloaded.</typeparam>
    internal interface ITaskManger<T>
    {
        /// <summary>
        /// Add download task .
        /// </summary>
        /// <param name="task"></param>
        void Add(T task);

        /// <summary>
        /// Delete download task .
        /// </summary>
        /// <param name="task"></param>
        void Remove(T task);
    }
}