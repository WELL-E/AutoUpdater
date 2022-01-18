namespace GeneralUpdate.Core.Download
{
    /// <summary>
    /// Download task interface.
    /// </summary>
    /// <typeparam name="T">'T' is the version information that needs to be downloaded.</typeparam>
    internal interface ITaskManger<ITask>
    {
        /// <summary>
        /// Add download task .
        /// </summary>
        /// <param name="task"></param>
        void EnPool(ITask task);

        /// <summary>
        /// Delete download task .
        /// </summary>
        /// <param name="task"></param>
        void DePool(ITask task);

        /// <summary>
        /// Start all task downloads .
        /// </summary>
        void Launch();
    }
}
