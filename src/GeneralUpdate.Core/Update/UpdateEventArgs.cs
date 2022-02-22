using GeneralUpdate.Common.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace GeneralUpdate.Core.Update
{
    public enum ProgressType {
        /// <summary>
        /// Check for updates
        /// </summary>
        Check,
        /// <summary>
        /// Download the update package
        /// </summary>
        Download,
        /// <summary>
        /// 更新文件
        /// </summary>
        UpdateFile,
        /// <summary>
        /// update completed
        /// </summary>
        Done,
        /// <summary>
        /// Update failed
        /// </summary>
        Fail
    }

    public class DownloadProgressChangedEventArgsEx
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

    #region Multi

    public class MultiDownloadStatisticsEventArgs : EventArgs
    {
        public object Version { get; set; }

        public DateTime Remaining { get; set; }

        public string Speed { get; set; }
    }

    public class MultiDownloadProgressChangedEventArgs : DownloadProgressChangedEventArgsEx
    {
        public ProgressType Type { get; set; }

        public object Version { get; set; }

        public string Message { get; set; }

        public double ProgressValue { get; set; }

        public MultiDownloadProgressChangedEventArgs(object version, ProgressType type,string message, long received = 0, long toReceive = 0, float progressPercentage = 0, object userState = null)
            : base(received, toReceive, progressPercentage, userState)
        {
            ProgressValue = progressPercentage;
            Version = version;
            Message = message;
            Type = type;
        }
    }

    public class MultiDownloadCompletedEventArgs : AsyncCompletedEventArgs
    {
        public object Version { get; set; }

        public MultiDownloadCompletedEventArgs(object version, Exception error, bool cancelled, object userState) : base(error, cancelled, userState)
        {
            Version = version;
        }
    }

    public class MultiAllDownloadCompletedEventArgs : EventArgs
    {
        public bool IsAllDownloadCompleted { get; set; }

        public IList<ValueTuple<UpdateVersion, string>> FailedVersions { get; set; }

        public MultiAllDownloadCompletedEventArgs(bool isAllDownloadCompleted, IList<ValueTuple<UpdateVersion, string>> failedVersions)
        {
            IsAllDownloadCompleted = isAllDownloadCompleted;
            FailedVersions = failedVersions;
        }
    }

    public class MultiDownloadErrorEventArgs : EventArgs
    {
        public Exception Exception { get; set; }

        public object Version { get; set; }

        public MultiDownloadErrorEventArgs(Exception exception, object updateVersion)
        {
            Exception = exception;
            Version = updateVersion;
        }
    }

    #endregion
}
