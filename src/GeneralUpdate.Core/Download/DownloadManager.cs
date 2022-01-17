using GeneralUpdate.Common.CustomAwaiter;
using GeneralUpdate.Common.Models;
using GeneralUpdate.Core.Update;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GeneralUpdate.Core.Download
{
    internal sealed class DownloadManager : AbstractTaskManager
    {
        #region Private Members

        /// <summary>
        /// Number of failed download tasks.
        /// </summary>
        //private int _failed;

        private IList<ValueTuple<UpdateVersion, string>> FailedVersions { get; set; }

        private string _path;
        private string _format;
        private int _timeOut;

        internal string Path { get => _path; }
        internal string Format { get => _format; }
        internal int TimeOut { get => _timeOut; }

        #endregion

        #region Constructors

        internal DownloadManager(string path, string format, int timeOut) 
        {
            _path = path;
            _format = format;
            _timeOut = timeOut;
            //FailedVersions = new List<ValueTuple<UpdateVersion, string>>();
        }

        #endregion

        #region Public Properties
         
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
                //TODO:下载完成之后按版本排序，实现IAwaiter异步方法
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

        #endregion

        #region Private Methods

        #endregion

    }
}