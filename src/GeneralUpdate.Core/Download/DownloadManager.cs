using GeneralUpdate.Common.Models;
using GeneralUpdate.Core.Update;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using GeneralUpdate.Common.CustomAwaiter;

namespace GeneralUpdate.Core.Download
{
    internal sealed class DownloadManager : AbstractTaskManager, IAwaiter
    {
        #region Private Members

        private IList<(object, string)> _failedVersions;
        private string _path;
        private string _format;
        private int _timeOut;

        #endregion

        #region Constructors

        internal DownloadManager(string path, string format, int timeOut)
        {
            _path = path;
            _format = format;
            _timeOut = timeOut;
            _failedVersions = new List<ValueTuple<UpdateVersion, string>>();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Record download exception information for all versions.
        /// object: is 'UpdateVersion' , string: is error infomation.
        /// </summary>
        internal IList<(object, string)> FailedVersions { get => _failedVersions; }
        internal string Path { get => _path; }
        internal string Format { get => _format; }
        internal int TimeOut { get => _timeOut; }

        public bool IsCompleted { get; }

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

        public override void Launch()
        {
            try
            {
                foreach (var task in DownloadTaskPool)
                {
                    task.Launch();
                }
                //TODO:实现IAwaiter异步方法,统计下载失败的版本号信息
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

        public void GetResult()
        {
            
        }

        public void OnCompleted(Action continuation)
        {
            continuation.Invoke();
        }

        #endregion

        #region Private Methods

        #endregion

    }
}