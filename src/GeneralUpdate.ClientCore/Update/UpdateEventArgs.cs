using GeneralUpdate.ClientCore.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace GeneralUpdate.ClientCore.Update
{
    public enum ProgressType {
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

    #region Muti

    public class MutiDownloadStatisticsEventArgs : EventArgs
    {
        public UpdateVersion Version { get; set; }

        public DateTime Remaining { get; set; }

        public string Speed { get; set; }
    }

    public class MutiDownloadProgressChangedEventArgs : DownloadProgressChangedEventArgsEx
    {
        public ProgressType Type { get; set; }

        public UpdateVersion Version { get; set; }

        public string Message { get; set; }

        public double ProgressValue { get; set; }

        public MutiDownloadProgressChangedEventArgs(UpdateVersion version, ProgressType type,string message, long received = 0, long toReceive = 0, float progressPercentage = 0, object userState = null)
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
        public UpdateVersion Version { get; set; }

        public MutiDownloadCompletedEventArgs(UpdateVersion version, Exception error, bool cancelled, object userState) : base(error, cancelled, userState)
        {
            Version = version;
        }
    }

    public class MutiAllDownloadCompletedEventArgs : EventArgs
    {
        public bool IsAllDownloadCompleted { get; set; }

        public IList<ValueTuple<UpdateVersion, string>> FailedVersions { get; set; }

        public MutiAllDownloadCompletedEventArgs(bool isAllDownloadCompleted, IList<ValueTuple<UpdateVersion, string>> failedVersions)
        {
            IsAllDownloadCompleted = isAllDownloadCompleted;
            FailedVersions = failedVersions;
        }
    }

    public class MutiDownloadErrorEventArgs : EventArgs
    {
        public Exception Exception { get; set; }

        public UpdateVersion Version { get; set; }

        public MutiDownloadErrorEventArgs(Exception exception, UpdateVersion updateVersion)
        {
            Exception = exception;
            Version = updateVersion;
        }
    }

    #endregion
}
