using GeneralUpdate.Core.Bootstrap;
using GeneralUpdate.Core.Models;
using GeneralUpdate.Core.Strategys;
using GeneralUpdate.Core.Utils;
using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

namespace GeneralUpdate.Core
{
    public class GeneralUpdateBootstrap : AbstractBootstrap<GeneralUpdateBootstrap, IStrategy>
    {
        /// <summary>
        /// 更新包名称
        /// </summary>
        public string PacketName { get; set; }

        /// <summary>
        /// 更新包下载路径
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 安装目录（更新包解压路径）
        /// </summary>
        public string InstallPath { get; set; }

        /// <summary>
        /// 更新包MD5码
        /// </summary>
        public string MD5 { get; set; }

        /// <summary>
        /// 最新版本
        /// </summary>
        public string NewVersion { get; set; }

        /// <summary>
        /// 当前版本
        /// </summary>
        public string CurrentVersion { get; set; }

        /// <summary>
        /// 更新日志网页地址
        /// </summary>
        public string UpdateLogUrl { get; set; }

        /// <summary>
        /// 是否强制更新
        /// </summary>
        public bool IsForcibly { get; set; }

        public string UpdateCheckUrl { get; set; }

        public GeneralUpdateBootstrap() : base()
        {
        }

        /// <summary>
        /// 配置远程地址
        /// </summary>
        /// <param name="args">
        /// 1.当前版本号（0.9.0.0）
        /// 2.升级版本号（1.0.0.0）
        /// 3.更新描述URL（https://github.com/WELL-E）
        /// 4.更新包文件的URL（http://localhost：9090/UpdateFile.zip）
        /// 5.更新了文件发布路径（E:\PlatformPath）
        /// 6.更新程序包文件MD5代码（2b406701f8ad92922feb537fc789561a）
        /// </param>
        public GeneralUpdateBootstrap RemoteAddress(string[] args)
        {
            CurrentVersion = args[0];
            NewVersion = args[1];
            UpdateLogUrl = args[2];
            Url = args[3];
            InstallPath = args[4].Replace("|", " ");
            MD5 = args[5];
            PacketName = Url.GetName(StringOption.Url);
            ValidateRemoteAddress(args);
            Init();
            return this;
        }

        private void Init() {
            var tempPath = FileUtil.GetTempDirectory(NewVersion);
            Packet = new UpdatePacket();
            Packet.Url = Url;
            Packet.InstallPath = InstallPath;
            Packet.Name = PacketName;
            Packet.MD5 = MD5;
            Packet.CurrentVersion = CurrentVersion;
            Packet.NewVersion = NewVersion;
            Packet.TempPath = $"{ tempPath }\\{PacketName}";
            base.Packet = this.Packet;
            base.UpdateCheckUrl = this.UpdateCheckUrl;
        }

        private void ValidateRemoteAddress(string[] args = null, int elementNum = 6) {

            if (args != null)
            {
                if (args.Length == 0)
                {
                    throw new NullReferenceException("Args does not contain any elements.");
                }

                if (args.Length > elementNum)
                {
                    throw new Exception($"The number of args cannot be greater than { elementNum }.");
                }
            }

            if (string.IsNullOrWhiteSpace(PacketName))
            {
                throw new NullReferenceException("packet name not set.");
            }

            if (string.IsNullOrWhiteSpace(Url))
            {
                throw new NullReferenceException("download path not set.");
            }

            if (string.IsNullOrWhiteSpace(InstallPath))
            {
                throw new NullReferenceException("install path not set.");
            }

            if (string.IsNullOrWhiteSpace(MD5))
            {
                throw new NullReferenceException("install path not set.");
            }
        }

        /// <summary>
        /// 配置远程地址
        /// </summary>
        /// <param name="remoteUrl">请求更新url 例如："https://api.com/GeneralUpdate?version=1.0.0.1"</param>
        public GeneralUpdateBootstrap RemoteAddress(string updateCheckUrl) {
            if (string.IsNullOrWhiteSpace(updateCheckUrl))
            {
                throw new NullReferenceException("Remote url not set.");
            }

            if (!IsURL(updateCheckUrl))
            {
                throw new NullReferenceException("The URL is not legal.");
            }

            UpdateCheckUrl = updateCheckUrl;
            InstallPath = System.Environment.CurrentDirectory;
            var pos = updateCheckUrl.LastIndexOf('=');
            CurrentVersion = updateCheckUrl.Substring(pos + 1);
            Init();
            return this;
        }

        private static bool IsURL(string url)
        {
            string check = @"((http|ftp|https)://)(([a-zA-Z0-9\._-]+\.[a-zA-Z]{2,6})|([0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}))(:[0-9]{1,4})*(/[a-zA-Z0-9\&%_\./-~-]*)?";
            Regex regex = new Regex(check);
            return regex.IsMatch(url);
        }

        public bool StartMain(string appName) {
            try
            {
                Process.Start($"{InstallPath}/{appName}.exe");
                Process.GetCurrentProcess().Kill();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
