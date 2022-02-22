using GeneralUpdate.Common.Models;
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
    public sealed class DownloadManager : AbstractTaskManager<UpdateVersion>
    {
        #region Private Members

        private string _path;
        private string _format;
        private int _timeOut;
        private IList<(object, string)> _failedVersions;
        private IList<ITask<UpdateVersion>> _downloadTaskPool;

        private IList<ITask<UpdateVersion>> DownloadTaskPool { get => _downloadTaskPool ?? (_downloadTaskPool = new List<ITask<UpdateVersion>>()); }

        #endregion

        #region Constructors

        /// <summary>
        ///  download manager constructors.
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
        /// object: is 'UpdateVersion' , string: is error information.
        /// </summary>
        internal IList<(object, string)> FailedVersions { get => _failedVersions; }
        internal string Path { get => _path; }

        //zip format
        internal string Format { get => _format; }

        internal int TimeOut { get => _timeOut; }

        public delegate void MutiAllDownloadCompletedEventHandler(object sender, MultiAllDownloadCompletedEventArgs e);
        public event MutiAllDownloadCompletedEventHandler MutiAllDownloadCompleted;

        public delegate void MutiDownloadProgressChangedEventHandler(object csender, MultiDownloadProgressChangedEventArgs e);
        public event MutiDownloadProgressChangedEventHandler MutiDownloadProgressChanged;

        public delegate void MutiAsyncCompletedEventHandler(object sender, MultiDownloadCompletedEventArgs e);
        public event MutiAsyncCompletedEventHandler MutiDownloadCompleted;

        public delegate void MutiDownloadErrorEventHandler(object sender, MultiDownloadErrorEventArgs e);
        public event MutiDownloadErrorEventHandler MutiDownloadError;

        public delegate void MutiDownloadStatisticsEventHandler(object sender, MultiDownloadStatisticsEventArgs e);
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
                    if (task is DownloadTask<UpdateVersion> downloadTask)
                    {
                        await downloadTask.Launch();
                    }
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

        public void OnMultiDownloadStatistics(object sender, MultiDownloadStatisticsEventArgs e)
        {
            if (MutiDownloadStatistics != null) this.MutiDownloadStatistics(sender, e);
        }

        public void OnMultiDownloadProgressChanged(object sender, MultiDownloadProgressChangedEventArgs e)
        {
            if (MutiDownloadProgressChanged != null) this.MutiDownloadProgressChanged(sender, e);
        }

        public void OnMultiAsyncCompleted(object sender, MultiDownloadCompletedEventArgs e)
        {
            if (MutiDownloadCompleted != null) this.MutiDownloadCompleted(sender, e);
        }

        public void OnMultiDownloadError(object sender, MultiDownloadErrorEventArgs e)
        {
            if (MutiDownloadError != null) this.MutiDownloadError(sender, e);

            _failedVersions.Add((e.Version,e.Exception.Message));
        }

        public override void Remove(ITask<UpdateVersion> task)
        {
            if (task != null && DownloadTaskPool.Contains(task)) DownloadTaskPool.Remove(task);
        }

        public override void Add(ITask<UpdateVersion> task)
        {
            if (task != null && !DownloadTaskPool.Contains(task)) DownloadTaskPool.Add(task);
        }

        #endregion

        #region Private Methods

        #endregion
    }
}