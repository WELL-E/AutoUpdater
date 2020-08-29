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
            var isVerify = VerifyFileMd5($"{_updatePacket.TempPath}", _updatePacket.MD5);
            if (!isVerify)
            {
                _eventAction(this, new Update.ProgressChangedEventArgs() { Type = ProgressType.Fail, Message = "Verify MD5 Error!" });
                return;
            }

            if (FileUtil.UnZip($"{ _updatePacket.TempPath }", _updatePacket.InstallPath, _eventAction))
            {
                var isDone = UpdateFiles();
                _eventAction(this, new Update.ProgressChangedEventArgs() { Type = isDone ? ProgressType.Done : ProgressType.Fail });

                if (isDone && !string.IsNullOrEmpty(_updatePacket.MainApp))
                {
                    //FileUtil.InitConfig("",new UpdateConfig());
                    StartMain();
                }
            }
            else
            {
                _eventAction(this, new Update.ProgressChangedEventArgs() { Type = ProgressType.Fail , Message = "UnPacket Error!" });
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
            catch (Exception ex)
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

                FileUtil.UpdateReg(
                    Registry.LocalMachine, 
                    FileUtil.SubKey, 
                    "DisplayVersion",
                    _updatePacket.NewVersion);

                if (File.Exists(_updatePacket.TempPath))
                {
                    File.Delete(_updatePacket.TempPath);
                }

                var dir = _updatePacket.TempPath.ExcludeName(StringOption.File);
                if (Directory.Exists(dir))
                {
                    Directory.Delete(dir);
                }
                
                FileUtil.Update32Or64Libs(_updatePacket.InstallPath);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
