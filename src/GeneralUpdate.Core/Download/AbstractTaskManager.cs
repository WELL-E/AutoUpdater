using System.Collections.Generic;

namespace GeneralUpdate.Core.Download
{
    /// <summary>
    /// Abstract task manager class.
    /// </summary>
    /// <typeparam name="T">'T' is the download task.</typeparam>
    internal abstract class AbstractTaskManager : ITaskManger<ITask>
    {
        #region Private Members

        private IList<ITask> _downloadTaskPool;

        #endregion

        #region Public Properties

        protected IList<ITask> DownloadTaskPool { get => _downloadTaskPool ?? (_downloadTaskPool = new List<ITask>()); }

        #endregion

        #region Public Methods

        public void DePool(ITask task)
        {
            if (task == null && DownloadTaskPool.Contains(task)) DownloadTaskPool.Remove(task);
        }

        public void EnPool(ITask task)
        {
            if (task == null && DownloadTaskPool.Contains(task)) DownloadTaskPool.Add(task);
        }

        public abstract void Launch();

        #endregion
    }
}
