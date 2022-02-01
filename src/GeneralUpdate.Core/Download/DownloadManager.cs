using GeneralUpdate.Core.Update;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace GeneralUpdate.Core.Download
{
    /// <summary>
    /// download task manager.
    /// </summary>
    /// <typeparam name="T">update version infomation.</typeparam>
    public sealed class DownloadManager<T> : AbstractTaskManager<T> where T : class
    {
        #region Private Members

        private string _path;
        private string _format;
        private int _timeOut;
        private IList<(object, string)> _failedVersions;
        private IList<ITask<T>> _downloadTaskPool;

        private IList<ITask<T>> DownloadTaskPool { get => _downloadTaskPool ?? (_downloadTaskPool = new List<ITask<T>>()); }

        #endregion

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="format"></param>
        /// <param name="timeOut"></param>
        internal DownloadManager(string path, string format, int timeOut)
        {
            _path = path;
            _format = format;
            _timeOut = timeOut;
            _failedVersions = new List<ValueTuple<object, string>>();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Record download exception information for all versions.
        /// object: is 'UpdateVersion' , string: is error infomation.
        /// </summary>
        internal IList<(object, string)> FailedVersions { get => _failedVersions; }
        internal string Path { get => _path; }

        //zip format
        internal string Format { get => _format; }

        
        internal int TimeOut { get => _timeOut; }

        public delegate void MutiAllDownloadCompletedEventHandler(object sender, MutiAllDownloadCompletedEventArgs e);
        public event MutiAllDownloadCompletedEventHandler MutiAllDownloadCompleted;

        public delegate void MutiDownloadProgressChangedEventHandler(object csender, MutiDownloadProgressChangedEventArgs e);
        public event MutiDownloadProgressChangedEventHandler MutiDownloadProgressChanged;

        public delegate void MutiAsyncCompletedEventHandler(object sender, MutiDownloadCompletedEventArgs e);
        public event MutiAsyncCompletedEventHandler MutiDownloadCompleted;

        public delegate void MutiDownloadErrorEventHandler(object sender, MutiDownloadErrorEventArgs e);
        public event MutiDownloadErrorEventHandler MutiDownloadError;

        public delegate void MutiDownloadStatisticsEventHandler(object sender, MutiDownloadStatisticsEventArgs e);
        public event MutiDownloadStatisticsEventHandler MutiDownloadStatistics;

        #endregion

        #region Public Methods

        /// <summary>
        /// launch update.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="AmbiguousMatchException"></exception>
        public async Task AsyncLaunch()
        {
            try
            {
                foreach (var task in DownloadTaskPool)
                {
                    var downloadTask = task as DownloadTask<T>;
                    await downloadTask.Launch();
                }
            }
            catch (ArgumentNullException ex)
            {
                throw new ArgumentNullException("Method 'GetMethod' in 'Launch' executes abnormally ! exception is 'ArgumentNullException'.", ex);
            }
            catch (AmbiguousMatchException ex)
            {
                throw new AmbiguousMatchException("Method 'GetMethod' in 'Launch' executes abnormally ! exception is 'AmbiguousMatchException'.", ex);
            }
        }

        public void OnMutiDownloadStatistics(object sender, MutiDownloadStatisticsEventArgs e)
        {
            if (MutiDownloadStatistics != null) this.MutiDownloadStatistics(sender, e);
        }

        public void OnMutiDownloadProgressChanged(object sender, MutiDownloadProgressChangedEventArgs e)
        {
            if (MutiDownloadProgressChanged != null) this.MutiDownloadProgressChanged(sender, e);
        }

        public void OnMutiAsyncCompleted(object sender, MutiDownloadCompletedEventArgs e)
        {
            if (MutiDownloadCompleted != null) this.MutiDownloadCompleted(sender, e);
        }

        public void OnMutiDownloadError(object sender, MutiDownloadErrorEventArgs e)
        {
            if (MutiDownloadError != null) this.MutiDownloadError(sender, e);

            _failedVersions.Add((e.Version,e.Exception.Message));
        }

        public override void DePool(ITask<T> task)
        {
            if (task != null && DownloadTaskPool.Contains(task)) DownloadTaskPool.Remove(task);
        }

        public override void EnPool(ITask<T> task)
        {
            if (task != null && !DownloadTaskPool.Contains(task)) DownloadTaskPool.Add(task);
        }

        #endregion

        #region Private Methods

        #endregion
    }
}