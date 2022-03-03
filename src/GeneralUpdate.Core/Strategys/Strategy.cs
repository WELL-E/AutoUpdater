using GeneralUpdate.Common.Models;
using GeneralUpdate.Core.Models;
using GeneralUpdate.Core.Update;
using GeneralUpdate.Core.Utils;
using GeneralUpdate.Zip;
using GeneralUpdate.Zip.Factory;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace GeneralUpdate.Core.Strategys
{
    public class DefaultStrategy : AbstractStrategy
    {
        #region Private Members

        protected UpdatePacket Packet { get; set; }
        protected Action<object, MutiDownloadProgressChangedEventArgs> ProgressEventAction { get; set; }
        protected Action<object, ExceptionEventArgs> ExceptionEventAction { get; set; }

        private OperationType _operationType;

        #endregion Private Members

        #region Public Methods

        public override void Create(IFile file, Action<object, MutiDownloadProgressChangedEventArgs> progressEventAction,
    Action<object, ExceptionEventArgs> exceptionEventAction)
        {
            Packet = (UpdatePacket)file;
            ProgressEventAction = progressEventAction;
            ExceptionEventAction = exceptionEventAction;
            _operationType = Packet.CompressFormat.Equals(".zip") ? OperationType.GZip : OperationType.G7z;
        }

        public override void Excute()
        {
            try
            {
                //if(Packet.AppType == 2) ConfigFactory.Instance.Scan();
                var updateVersions = Packet.UpdateVersions;
                updateVersions = updateVersions.OrderBy(x => x.PubTime).ToList();
                foreach (var version in updateVersions)
                {
                    var zipFilePath = $"{Packet.TempPath}{ version.Name }{ Packet.CompressFormat }";
                    var isVerify = VerifyFileMd5(zipFilePath, version.MD5);
                    if (!isVerify)
                    {
                        var eventArgs = new MutiDownloadProgressChangedEventArgs(version, ProgressType.Fail, "Verify MD5 error!");
                        ProgressEventAction(this, eventArgs);
                        throw new Exception($"The update package MD5 code is inconsistent ! Version-{ version.Version }  MD5-{ version.MD5 } .");
                    }
                    if (UnZip(version, zipFilePath, Packet.InstallPath))
                    {
                        version.IsUnZip = true;
                        var versionArgs = new UpdateVersion(version.MD5, version.PubTime, version.Version, null, version.Name);
                        var message = version.IsUnZip ? "Update completed." : "Update failed!";
                        var type = version.IsUnZip ? ProgressType.Done : ProgressType.Fail;
                        var eventArgs = new MutiDownloadProgressChangedEventArgs(versionArgs, type, message);
                        ProgressEventAction(this, eventArgs);
                    }
                }
                CheckAllIsUnZip(updateVersions);
                Dirty();
                //if (Packet.AppType == 2) ConfigFactory.Instance.Deploy();
                StartApp(Packet.AppName);
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
                {
                    Process.Start("explorer.exe", Packet.UpdateLogUrl);
                }
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

        protected void CheckAllIsUnZip(List<UpdateVersion> versions)
        {
            foreach (var version in versions)
            {
                if (!version.IsUnZip) throw new Exception($"Failed to decompress the compressed package! Version-{ version.Version }  MD5-{ version.MD5 } .");
            }
        }

        /// <summary>
        /// UnZip
        /// </summary>
        /// <param name="zipfilepath"></param>
        /// <param name="unzippath"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        protected bool UnZip(UpdateVersion versionInfo, string zipfilepath, string unzippath)
        {
            try
            {
                bool isComplated = false;
                var generalZipFactory = new GeneralZipFactory();
                generalZipFactory.UnZipProgress += (sender, e) =>
                {
                    if (ProgressEventAction == null) return;
                    var version = new UpdateVersion(versionInfo.MD5, versionInfo.PubTime, versionInfo.Version, null, versionInfo.Name);
                    var eventArgs = new MutiDownloadProgressChangedEventArgs(version, ProgressType.Updatefile, "Updatting file...");
                    ProgressEventAction(this, eventArgs);
                };
                generalZipFactory.Completed += (sender, e) => isComplated = true;
                generalZipFactory.CreatefOperate(_operationType, zipfilepath, unzippath, false, Packet.CompressEncoding).
                    UnZip();
                return isComplated;
            }
            catch (Exception ex)
            {
                if (ExceptionEventAction != null) ExceptionEventAction(this, new ExceptionEventArgs(ex));
                return false;
            }
        }

        protected bool VerifyFileMd5(string fileName, string md5)
        {
            var packetMD5 = FileUtil.GetFileMD5(fileName);
            if (md5.ToUpper().Equals(packetMD5.ToUpper())) return true;
            return false;
        }

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