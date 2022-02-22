﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using GeneralUpdate.Common.Models;
using GeneralUpdate.Core.Models;
using GeneralUpdate.Core.Update;
using GeneralUpdate.Core.Utils;
using GeneralUpdate.Zip.GZip;

namespace GeneralUpdate.Core.Strategies
{
    public class DefaultStrategy : AbstractStrategy
    {
        protected UpdatePacket Packet { get; set; }
        protected Action<object, MultiDownloadProgressChangedEventArgs> ProgressEventAction { get; set; }
        protected Action<object, ExceptionEventArgs> ExceptionEventAction { get; set; }

        public override void Create(IFile file, Action<object, MultiDownloadProgressChangedEventArgs> progressEventAction, 
            Action<object, ExceptionEventArgs> exceptionEventAction)
        {
            Packet = (UpdatePacket)file;
            ProgressEventAction = progressEventAction;
            ExceptionEventAction = exceptionEventAction;
        }

        public override void Execute()
        {
            try
            {
                var updateVersions = Packet.UpdateVersions;
                updateVersions = updateVersions.OrderBy(x => x.PubTime).ToList();
                foreach (var version in updateVersions)
                {
                    var zipFilePath = $"{Packet.TempPath}{ version.Name }{ Packet.Format }";
                    var isVerify = VerifyFileMd5(zipFilePath, version.MD5);
                    if (!isVerify)
                    {
                        var eventArgs = new MultiDownloadProgressChangedEventArgs(null, ProgressType.Fail, "Verify MD5 error!");
                        ProgressEventAction(this, eventArgs);
                        continue;
                    }

                    if (UnZip(version, zipFilePath, Packet.InstallPath))
                    {
                        version.IsUnZip = true;
                        var versionArgs = new UpdateVersion(version.MD5,
                            version.PubTime,
                            version.Version,
                            null,
                            version.Name);

                        var message = version.IsUnZip ? "update completed." : "Update failed!";
                        var type = version.IsUnZip ? ProgressType.Done : ProgressType.Fail;
                        var eventArgs = new MultiDownloadProgressChangedEventArgs(versionArgs, type, message);
                        ProgressEventAction(this, eventArgs);
                    }
                }
                var isDone = CheckAllIsUnZip(updateVersions);
                if (isDone) 
                {
                    UpdateFiles();
                    StartApp(Packet.AppName);
                }
                else
                {
                    Error(new Exception($"Failed to decompress the compressed package!"));
                } 
            }
            catch (Exception ex)
            {
                Error(ex);
            }
        }

        private void Error(Exception ex) 
        {
            if (ExceptionEventAction != null)
                ExceptionEventAction(this, new ExceptionEventArgs(ex));
        }

        protected bool CheckAllIsUnZip(List<Common.Models.UpdateVersion> versions) 
        {
            foreach (var version in versions)
            {
                if (!version.IsUnZip)
                {
                    return false;
                }
            }
            return true;
        }

        protected override bool StartApp(string appName)
        {
            try
            {
                if (!string.IsNullOrEmpty(Packet.UpdateLogUrl))
                {
                    Process.Start("explorer.exe", Packet.UpdateLogUrl);
                }
                Process.Start($"{Packet.InstallPath}\\{appName}.exe");
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                Process.GetCurrentProcess().Kill();
            }
        }

        /// <summary>
        /// UnZip
        /// </summary>
        /// <param name="zipfilepath"></param>
        /// <param name="unzippath"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        protected bool UnZip(Common.Models.UpdateVersion versionInfo, string zipfilepath, string unzippath)
        {
            try
            {
                GeneralZip gZip = new GeneralZip();
                gZip.UnZipProgress += (sender,e) => 
                {
                    var version = new UpdateVersion(versionInfo.MD5, 
                        versionInfo.PubTime, 
                        versionInfo.Version, 
                        null,
                        versionInfo.Name);
                    var eventArgs = new MultiDownloadProgressChangedEventArgs(version, ProgressType.UpdateFile, "Updatting file...");

                    if (ProgressEventAction != null) ProgressEventAction(this, eventArgs);
                };
                bool isUnZip = gZip.UnZip(zipfilepath, unzippath);
                return isUnZip;
            }
            catch (Exception ex)
            {
                if (ExceptionEventAction != null)
                    ExceptionEventAction(this, new ExceptionEventArgs(ex));
                return false;
            }
        }

        protected bool VerifyFileMd5(string fileName, string md5)
        {
            var packetMD5 = FileUtil.GetFileMD5(fileName);

            if (md5.ToUpper().Equals(packetMD5.ToUpper()))
            {
                return true;
            }
            return false;
        }

        protected bool UpdateFiles()
        {
            try
            {
                //FileUtil.DirectoryCopy(
                //    UpdatePacket.TempPath,
                //    UpdatePacket.InstallPath,
                //    true, 
                //    true, 
                //    o => UpdatePacket.Name = o);

                if (File.Exists(Packet.TempPath))
                {
                    File.Delete(Packet.TempPath);
                }

                var dir = Packet.TempPath.ExcludeName(StringOption.File);
                if (Directory.Exists(dir))
                {
                    Directory.Delete(dir,true);
                }
                
                FileUtil.Update32Or64Libs(Packet.InstallPath);

                return true;
            }
            catch (Exception ex)
            {
                if (ExceptionEventAction != null)
                    ExceptionEventAction(this, new ExceptionEventArgs(ex));
                return false;
            }
        }
    }
}