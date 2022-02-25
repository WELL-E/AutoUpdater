using GeneralUpdate.Common.CustomAwaiter;
using GeneralUpdate.Core.Update;
using GeneralUpdate.Core.Utils;
using System;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;

namespace GeneralUpdate.Core.Download
{
    /// <summary>
    /// Download task class.
    /// </summary>
    /// <typeparam name="TVersion">'T' is the version information that needs to be downloaded.</typeparam>
    public sealed class DownloadTask<TVersion> : AbstractTask<TVersion>, IAwaiter<DownloadTask<TVersion>> where TVersion : class
    {
        #region Private Members

        private Exception _exception;
        private DownloadManager<TVersion> _manager;
        private const int DEFAULT_DELTA = 1048576;//1024*1024

        public TVersion _version { get; private set; }

        public bool IsCompleted { get; private set; }

        #endregion Private Members

        #region Constructors

        public DownloadTask(DownloadManager<TVersion> manger, TVersion version)
        {
            _manager = manger;
            _version = version;
        }

        #endregion Constructors

        #region Public Methods

        public async Task Launch()
        {
            try
            {
                var url = GetPropertyValue<string>(_version, "Url");
                var name = GetPropertyValue<string>(_version, "Name");
                InitTimeOut(_manager.TimeOut);
                InitStatisticsEvent();
                InitProgressEvent();
                InitCompletedEvent();
                var installPath = $"{ _manager.Path }{ name }{_manager.Format}";
                DownloadFileRange(url, installPath, null);
                await this;
            }
            catch (Exception ex)
            {
                throw _exception = new Exception("'Launch' The method executes abnormally !", ex);
            }
        }

        public DownloadTask<TVersion> GetResult()
        {
            if (_exception != null)
            {
                ExceptionDispatchInfo.Capture(_exception).Throw();
            }
            return this;
        }

        public void OnCompleted(Action continuation)
        {
            if (IsCompleted)
            {
                continuation?.Invoke();
            }
        }

        public DownloadTask<TVersion> GetAwaiter()
        {
            return this;
        }

        public async Task AsTask(DownloadTask<TVersion> awaiter)
        {
            await awaiter;
        }

        #endregion Public Methods

        #region Private Methods

        private void InitStatisticsEvent()
        {
            if (SpeedTimer != null) return;

            SpeedTimer = new Timer(_ =>
            {
                try
                {
                    var interval = DateTime.Now - StartTime;

                    var downLoadSpeed = interval.Seconds < 1
                        ? StatisticsUtil.ToUnit(ReceivedBytes - BeforBytes)
                        : StatisticsUtil.ToUnit(ReceivedBytes - BeforBytes / interval.Seconds);

                    var size = (TotalBytes - ReceivedBytes) / DEFAULT_DELTA;
                    var remainingTime = new DateTime().AddSeconds(Convert.ToDouble(size));

                    _manager.OnMutiDownloadStatistics(this, new MutiDownloadStatisticsEventArgs() { Version = _version, Remaining = remainingTime, Speed = downLoadSpeed });

                    StartTime = DateTime.Now;
                    BeforBytes = ReceivedBytes;
                }
                catch (Exception exception)
                {
                    _manager.OnMutiDownloadError(this, new MutiDownloadErrorEventArgs(exception, _version));
                }
            }, null, 0, 1000);
        }

        private void InitProgressEvent()
        {
            DownloadProgressChangedEx += new DownloadProgressChangedEventHandlerEx((sender, e) =>
            {
                try
                {
                    ReceivedBytes = e.BytesReceived;
                    TotalBytes = e.TotalBytesToReceive;

                    var eventArgs = new MutiDownloadProgressChangedEventArgs(_version,
                        ProgressType.Donwload,
                        string.Empty,
                        e.BytesReceived / DEFAULT_DELTA,
                        e.TotalBytesToReceive / DEFAULT_DELTA,
                        e.ProgressPercentage,
                        e.UserState);

                    _manager.OnMutiDownloadProgressChanged(this, eventArgs);
                }
                catch (Exception exception)
                {
                    _manager.OnMutiDownloadError(this, new MutiDownloadErrorEventArgs(exception, _version));
                }
            });
        }

        private void InitCompletedEvent()
        {
            DownloadFileCompletedEx += new AsyncCompletedEventHandlerEx((sender, e) =>
            {
                try
                {
                    if (SpeedTimer != null)
                    {
                        SpeedTimer.Dispose();
                        SpeedTimer = null;
                    }

                    var eventArgs = new MutiDownloadCompletedEventArgs(_version, e.Error, e.Cancelled, e.UserState);
                    _manager.OnMutiAsyncCompleted(this, eventArgs);

                    Dispose();
                }
                catch (Exception exception)
                {
                    _manager.FailedVersions.Add(new ValueTuple<object, string> { });
                    _manager.OnMutiDownloadError(this, new MutiDownloadErrorEventArgs(exception, _version));
                }
                finally
                {
                    IsCompleted = true;
                }
            });
        }

        private R GetPropertyValue<R>(TVersion entity, string propertyName)
        {
            R result = default(R);
            Type entityType = typeof(TVersion);
            try
            {
                PropertyInfo info = entityType.GetProperty(propertyName);
                result = (R)info.GetValue(entity);
            }
            catch (ArgumentNullException ex)
            {
                throw _exception = new ArgumentNullException("'GetPropertyValue' The method executes abnormally !", ex);
            }
            catch (AmbiguousMatchException ex)
            {
                throw _exception = new AmbiguousMatchException("'GetPropertyValue' The method executes abnormally !", ex);
            }
            return result;
        }

        #endregion Private Methods
    }
}