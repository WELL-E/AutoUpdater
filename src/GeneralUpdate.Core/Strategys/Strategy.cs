using GeneralUpdate.Core.Models;
using GeneralUpdate.Core.Update;
using GeneralUpdate.Core.Utils;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;

namespace GeneralUpdate.Core.Strategys
{
    /// <summary>
    /// 默认策略实现
    /// </summary>
    public class DefultStrategy : AbstractStrategy, IStrategy
    {
        private UpdatePacket _updatePacket;
        private Action<object, Update.ProgressChangedEventArgs> _eventAction;

        public void Create(IFile file, Action<object, Update.ProgressChangedEventArgs> eventAction)
        {
            _updatePacket = (UpdatePacket)file;
            _eventAction = eventAction;
        }

        public void Excute()
        {
            string tempPath = FileUtil.GetTempDirectory(_updatePacket.NewVersion);
            try
            {
                var isVerify = VerifyFileMd5($"{_updatePacket.TempPath}", _updatePacket.MD5);
                if (!isVerify)
                {
                    _eventAction.BeginInvoke(this, new Update.ProgressChangedEventArgs() { Type = ProgressType.Fail, Message = "Verify MD5 Error!" }, null, null);
                    return;
                }

                string tempBackups_new = $"{tempPath}\\backups_new";
                string tempBackups = $"{tempPath}\\backups";
                IncrementalFileUtil.Instance.GetOldFileinfo(_updatePacket.InstallPath);

                bool isCreate = FileUtil.CreateFloder(tempBackups_new);
                if (isCreate)
                {
                    bool isUnZip = FileUtil.UnZip($"{ _updatePacket.TempPath }", tempBackups_new, null);
                    bool isCreate_bkps = FileUtil.CreateFloder(tempBackups);
                    if (isUnZip && isCreate_bkps)
                    {
                        IncrementalFileUtil.Instance.GetNewFileinfo(tempBackups_new);
                        IncrementalFileUtil.Instance.GetIncrementalFiles();
                        IncrementalFileUtil.Instance.Backups(tempBackups);
                    }
                }
                
                if (FileUtil.UnZip($"{ _updatePacket.TempPath }", _updatePacket.InstallPath, _eventAction))
                {
                    var isDone = UpdateFiles();
                    _eventAction.BeginInvoke(this, new Update.ProgressChangedEventArgs() { Type = isDone ? ProgressType.Done : ProgressType.Fail }, null, null);

                    if (isDone && !string.IsNullOrEmpty(_updatePacket.MainApp))
                    {
                        StartMain();
                    }
                }
                else
                {
                    _eventAction.BeginInvoke(this, new Update.ProgressChangedEventArgs() { Type = ProgressType.Fail, Message = "UnPacket Error!" }, null, null);
                }
            }
            catch (Exception)
            {
                IncrementalFileUtil.Instance.RollBack(_updatePacket.InstallPath, tempPath);
                _eventAction.BeginInvoke(this, new Update.ProgressChangedEventArgs() { Type = ProgressType.Fail, Message = "Update fail ,Rollback operation performed." }, null, null);
            }
        }

        /// <summary>
        /// 启动主程序
        /// </summary>
        protected bool StartMain()
        {
            try
            {
                Process.Start($"{_updatePacket.InstallPath}\\{_updatePacket.MainApp}.exe");
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

        public bool UpdateFiles()
        {
            try
            {
                FileUtil.DirectoryCopy(
                    _updatePacket.TempPath,
                    _updatePacket.InstallPath,
                    true, 
                    true, 
                    o => _updatePacket.Name = o);

                //FileUtil.UpdateReg(
                //    Registry.LocalMachine, 
                //    FileUtil.SubKey, 
                //    "DisplayVersion",
                //    _updatePacket.NewVersion);

                if (File.Exists(_updatePacket.TempPath))
                {
                    File.Delete(_updatePacket.TempPath);
                }

                var dir = _updatePacket.TempPath.ExcludeName(StringOption.File);
                if (Directory.Exists(dir))
                {
                    Directory.Delete(dir,true);
                }
                
                FileUtil.Update32Or64Libs(_updatePacket.InstallPath);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
