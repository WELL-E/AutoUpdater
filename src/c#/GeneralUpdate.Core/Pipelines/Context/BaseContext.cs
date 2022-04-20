﻿using GeneralUpdate.Core.Models;
using GeneralUpdate.Core.Update;
using System;
using System.Text;

namespace GeneralUpdate.Core.Pipelines.Context
{
    /// <summary>
    /// Pipeline common content.
    /// </summary>
    public class BaseContext
    {
        private Action<object, MutiDownloadProgressChangedEventArgs> ProgressEventAction { get; set; }

        private Action<object, ExceptionEventArgs> ExceptionEventAction { get; set; }

        public UpdateVersion Version { get; set; }

        public string ZipfilePath { get; set; }

        public string TargetPath { get; set; }

        public string SourcePath { get; set; }

        public string Format { get; set; }

        public Encoding Encoding { get; set; }

        public BaseContext(Action<object, MutiDownloadProgressChangedEventArgs> progressEventAction,
            Action<object, ExceptionEventArgs> exceptionEventAction, 
            UpdateVersion version, string zipfilePath, string targetPath, string sourcePath, string format, Encoding encoding)
        {
            ProgressEventAction = progressEventAction;
            ExceptionEventAction = exceptionEventAction;
            Version = version;
            ZipfilePath = zipfilePath;
            TargetPath = targetPath;
            SourcePath = sourcePath;
            Format = format;
            Encoding = encoding;
        }

        public void OnProgressEventAction(object handle, ProgressType type, string message)
        {
            if (ProgressEventAction == null) return;
            var eventArgs = 
                new MutiDownloadProgressChangedEventArgs(new UpdateVersion(Version.MD5, Version.PubTime, Version.Version, null, Version.Name), type, message);
            ProgressEventAction(handle, eventArgs);
        }

        public void OnExceptionEventAction(object handle, Exception exception)
        {
            if (ExceptionEventAction != null) ExceptionEventAction(handle, new ExceptionEventArgs(exception));
        }
    }
}