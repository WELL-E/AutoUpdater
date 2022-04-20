using GeneralUpdate.Core.Events;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Reflection;
using System.Threading.Tasks;
using static GeneralUpdate.Core.Events.CommonEvent;

namespace GeneralUpdate.Core.Download
{
    /// <summary>
    /// download task manager.
    /// </summary>
    /// <typeparam name="T">update version infomation.</typeparam>
    public sealed class DownloadManager<TVersion> : AbstractTaskManager<TVersion>
    {
        #region Private Members

        private string _path;
        private string _format;
        private int _timeOut;
        private IList<(object, string)> _failedVersions;
        private ImmutableList<ITask<TVersion>>.Builder _downloadTasksBuilder;
        private ImmutableList<ITask<TVersion>> _downloadTasks;

        #endregion Private Members

        #region Constructors

        /// <summary>
        ///  download manager constructors.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="format"></param>
        /// <param name="timeOut"></param>
        public DownloadManager(string path, string format, int timeOut)
        {
            _path = path;
            _format = format;
            _timeOut = timeOut;
            _failedVersions = new List<ValueTuple<object, string>>();
            _downloadTasksBuilder = ImmutableList.Create<ITask<TVersion>>().ToBuilder();
        }

        #endregion Constructors

        #region Public Properties

        /// <summary>
        /// Record download exception information for all versions.
        /// object: is 'UpdateVersion' , string: is error infomation.
        /// </summary>
        public IList<(object, string)> FailedVersions { get => _failedVersions; }

        public string Path { get => _path; }

        public string Format { get => _format; }

        public int TimeOut { get => _timeOut; }

        public ImmutableList<ITask<TVersion>> DownloadTasks { get => _downloadTasks ?? (_downloadTasksBuilder.ToImmutable()); private set => _downloadTasks = value; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// launch update.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="AmbiguousMatchException"></exception>
        public void LaunchTaskAsync()
        {
            try
            {
                var downloadTasks = new List<Task>();
                foreach (var task in DownloadTasks)
                {
                    var downloadTask = (task as DownloadTask<TVersion>);
                    downloadTasks.Add(downloadTask.Launch());
                }
                Task.WaitAll(downloadTasks.ToArray());
                GeneralEventManager.Instance.Dispatch<MutiAllDownloadCompletedEventHandler>(this, new MutiAllDownloadCompletedEventArgs(true, _failedVersions));
            }
            catch (ObjectDisposedException ex)
            {
                GeneralEventManager.Instance.Dispatch<MutiAllDownloadCompletedEventHandler>(this, new MutiAllDownloadCompletedEventArgs(false, _failedVersions));
                throw new ArgumentNullException("Method 'GetMethod' in 'Launch' executes abnormally ! exception is 'ObjectDisposedException'.", ex);
            }
            catch (AggregateException ex)
            {
                GeneralEventManager.Instance.Dispatch<MutiAllDownloadCompletedEventHandler>(this, new MutiAllDownloadCompletedEventArgs(false, _failedVersions));
                throw new ArgumentNullException("Method 'GetMethod' in 'Launch' executes abnormally ! exception is 'AggregateException'.", ex);
            }
            catch (ArgumentNullException ex)
            {
                GeneralEventManager.Instance.Dispatch<MutiAllDownloadCompletedEventHandler>(this, new MutiAllDownloadCompletedEventArgs(false, _failedVersions));
                throw new ArgumentNullException("Method 'GetMethod' in 'Launch' executes abnormally ! exception is 'ArgumentNullException'.", ex);
            }
            catch (AmbiguousMatchException ex)
            {
                GeneralEventManager.Instance.Dispatch<MutiAllDownloadCompletedEventHandler>(this, new MutiAllDownloadCompletedEventArgs(false, _failedVersions));
                throw new AmbiguousMatchException("Method 'GetMethod' in 'Launch' executes abnormally ! exception is 'AmbiguousMatchException'.", ex);
            }
        }

        public override void Remove(ITask<TVersion> task)
        {
            if (task != null && _downloadTasksBuilder.Contains(task)) _downloadTasksBuilder.Remove(task);
        }

        public override void Add(ITask<TVersion> task)
        {
            if (task != null && !_downloadTasksBuilder.Contains(task)) _downloadTasksBuilder.Add(task);
        }

        #endregion Public Methods
    }
}