﻿using GeneralUpdate.Core.Events;
using GeneralUpdate.Core.Models;
using GeneralUpdate.Core.Pipelines;
using GeneralUpdate.Core.Pipelines.Context;
using GeneralUpdate.Core.Pipelines.Middleware;
using GeneralUpdate.Core.Update;
using GeneralUpdate.Core.Utils;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GeneralUpdate.Core.Strategys
{
    public class DefaultStrategy : AbstractStrategy
    {
        #region Private Members

        private const string PATCHS = "patchs";

        protected UpdatePacket Packet { get; set; }
        protected Action<object, MutiDownloadProgressChangedEventArgs> ProgressEventAction { get; set; }
        protected Action<object, ExceptionEventArgs> ExceptionEventAction { get; set; }

        #endregion Private Members

        #region Public Methods

        public override void Create(IFile file, Action<object, MutiDownloadProgressChangedEventArgs> progressEventAction,
    Action<object, ExceptionEventArgs> exceptionEventAction)
        {
            Packet = (UpdatePacket)file;
            ProgressEventAction = progressEventAction;
            ExceptionEventAction = exceptionEventAction;
        }

        public override void Excute()
        {
            try
            {
                Task.Run(async () =>
                {
                    var updateVersions = Packet.UpdateVersions.OrderBy(x => x.PubTime).ToList();
                    if (updateVersions != null && updateVersions.Count > 0) 
                    {
                        var patchPath = FileUtil.GetTempDirectory(PATCHS);
                        foreach (var version in updateVersions)
                        {
                            var zipFilePath = $"{Packet.TempPath}{ version.Name }{ Packet.Format }";
                            var pipelineBuilder = new PipelineBuilder<BaseContext>(new BaseContext(ProgressEventAction, ExceptionEventAction, version, zipFilePath, patchPath, Packet.InstallPath, Packet.Format, Packet.Encoding)).
                                UseMiddleware<MD5Middleware>().
                                UseMiddleware<ZipMiddleware>().
                                UseMiddleware<ConfigMiddleware>().
                                UseMiddleware<PatchMiddleware>();
                            await pipelineBuilder.Launch();
                        }
                    }
                    Dirty();
                    StartApp(Packet.AppName);
                });
            }
            catch (Exception ex)
            {
                Error(ex);
                return;
            }
        }

        protected override bool StartApp(string appName)
        {
            try
            {
                if (!string.IsNullOrEmpty(Packet.UpdateLogUrl))
                    Process.Start("explorer.exe", Packet.UpdateLogUrl);
                Process.Start($"{Packet.InstallPath}\\{appName}.exe");
                return true;
            }
            catch (Exception ex)
            {
                Error(ex);
                return false;
            }
            finally
            {
                Process.GetCurrentProcess().Kill();
            }
        }

        #endregion Public Methods

        #region Private Methods

        private void Error(Exception ex)
        { if (ExceptionEventAction != null) ExceptionEventAction(this, new ExceptionEventArgs(ex)); }

        /// <summary>
        /// Remove update redundant files.
        /// </summary>
        /// <returns></returns>
        private bool Dirty()
        {
            try
            {
                if (File.Exists(Packet.TempPath)) File.Delete(Packet.TempPath);
                var dirPath = Path.GetDirectoryName(Packet.TempPath);
                if (Directory.Exists(dirPath)) Directory.Delete(dirPath, true);
                return true;
            }
            catch (Exception ex)
            {
                if (ExceptionEventAction != null)
                    ExceptionEventAction(this, new ExceptionEventArgs(ex));
                return false;
            }
        }

        #endregion Private Methods
    }
}