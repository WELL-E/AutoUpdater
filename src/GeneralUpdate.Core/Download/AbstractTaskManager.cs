using GeneralUpdate.Core.Update;
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

        //public delegate void MutiAllDownloadCompletedEventHandler(object sender, MutiAllDownloadCompletedEventArgs e);
        //public event MutiAllDownloadCompletedEventHandler MutiAllDownloadCompleted;

        //public delegate void MutiDownloadProgressChangedEventHandler(object csender, MutiDownloadProgressChangedEventArgs e);
        //public event MutiDownloadProgressChangedEventHandler MutiDownloadProgressChanged;

        //public delegate void MutiAsyncCompletedEventHandler(object sender, MutiDownloadCompletedEventArgs e);
        //public event MutiAsyncCompletedEventHandler MutiDownloadCompleted;

        //public delegate void MutiDownloadErrorEventHandler(object sender, MutiDownloadErrorEventArgs e);
        //public event MutiDownloadErrorEventHandler MutiDownloadError;

        //public delegate void MutiDownloadStatisticsEventHandler(object sender, MutiDownloadStatisticsEventArgs e);
        //public event MutiDownloadStatisticsEventHandler MutiDownloadStatistics;

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
