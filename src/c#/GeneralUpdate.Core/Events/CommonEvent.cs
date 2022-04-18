using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace GeneralUpdate.Core.Events
{
    public enum ProgressType
    {
        /// <summary>
        /// Check for updates
        /// </summary>
        Check,

        /// <summary>
        /// Download the update package
        /// </summary>
        Donwload,

        /// <summary>
        /// 更新文件
        /// </summary>
        Updatefile,

        /// <summary>
        /// update completed
        /// </summary>
        Done,

        /// <summary>
        /// Update failed
        /// </summary>
        Fail,

        /// <summary>
        /// Update config
        /// </summary>
        Config,

        /// <summary>
        /// Update patch
        /// </summary>
        Patch,

        MD5
    }

    public class CommonEvent
    {
        public delegate void MutiAllDownloadCompletedEventHandler(object sender, MutiAllDownloadCompletedEventArgs e);

        public event MutiAllDownloadCompletedEventHandler MutiAllDownloadCompleted;

        public delegate void MutiDownloadProgressChangedEventHandler(object sender, MutiDownloadProgressChangedEventArgs e);

        public event MutiDownloadProgressChangedEventHandler MutiDownloadProgressChanged;

        public delegate void MutiAsyncCompletedEventHandler(object sender, MutiDownloadCompletedEventArgs e);

        public event MutiAsyncCompletedEventHandler MutiDownloadCompleted;

        public delegate void MutiDownloadErrorEventHandler(object sender, MutiDownloadErrorEventArgs e);

        public event MutiDownloadErrorEventHandler MutiDownloadError;

        public delegate void MutiDownloadStatisticsEventHandler(object sender, MutiDownloadStatisticsEventArgs e);

        public event MutiDownloadStatisticsEventHandler MutiDownloadStatistics;

        public delegate void ExceptionEventHandler(object sender, ExceptionEventArgs e);

        public event ExceptionEventHandler Exception;
    }

    public class DownloadProgressChangedEventArgsEx : EventArgs
    {
        public long BytesReceived { get; private set; }

        public long TotalBytesToReceive { get; private set; }

        public float ProgressPercentage { get; private set; }

        public object UserState { get; set; }

        public DownloadProgressChangedEventArgsEx(long received, long toReceive, float progressPercentage, object userState)
        {
            BytesReceived = received;
            TotalBytesToReceive = toReceive;
            ProgressPercentage = progressPercentage;
            UserState = userState;
        }
    }

    public class ExceptionEventArgs : EventArgs
    {
        public Exception Exception { get; set; }

        public ExceptionEventArgs(Exception exception)
        {
            Exception = exception;
        }
    }

    #region Muti

    public class MutiDownloadStatisticsEventArgs : EventArgs
    {
        public object Version { get; set; }

        public DateTime Remaining { get; set; }

        public string Speed { get; set; }
    }

    public class MutiDownloadProgressChangedEventArgs : DownloadProgressChangedEventArgsEx
    {
        public ProgressType Type { get; set; }

        public object Version { get; set; }

        public string Message { get; set; }

        public double ProgressValue { get; set; }

        public MutiDownloadProgressChangedEventArgs(object version, ProgressType type, string message, long received = 0, long toReceive = 0, float progressPercentage = 0, object userState = null)
            : base(received, toReceive, progressPercentage, userState)
        {
            ProgressValue = progressPercentage;
            Version = version;
            Message = message;
            Type = type;
        }
    }

    public class MutiDownloadCompletedEventArgs : AsyncCompletedEventArgs
    {
        public object Version { get; set; }

        public MutiDownloadCompletedEventArgs(object version, Exception error, bool cancelled, object userState) : base(error, cancelled, userState)
        {
            Version = version;
        }
    }

    public class MutiAllDownloadCompletedEventArgs : EventArgs
    {
        public bool IsAllDownloadCompleted { get; set; }

        public IList<ValueTuple<object, string>> FailedVersions { get; set; }

        public MutiAllDownloadCompletedEventArgs(bool isAllDownloadCompleted, IList<ValueTuple<object, string>> failedVersions)
        {
            IsAllDownloadCompleted = isAllDownloadCompleted;
            FailedVersions = failedVersions;
        }
    }

    public class MutiDownloadErrorEventArgs : EventArgs
    {
        public Exception Exception { get; set; }

        public object Version { get; set; }

        public MutiDownloadErrorEventArgs(Exception exception, object updateVersion)
        {
            Exception = exception;
            Version = updateVersion;
        }
    }

    #endregion Muti
}
