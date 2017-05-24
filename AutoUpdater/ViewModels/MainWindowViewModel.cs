/*
 * Copyright (c) well-e.  All rights reserved.
 */

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using AutoUpdater.Models;
using AutoUpdater.Utils;
using Microsoft.Win32;

namespace AutoUpdater.ViewModels
{
    internal class MainWindowViewModel : BindableBase
    {
        #region Private Members
        private const string SubKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{655A2DE6-C9A3-432E-951B-D773791C2653}_is1";

        private string _statusDescription;
        private string _downLoadSpeed = "0";
        private string _installFileName;

        private bool _isDownloading;
        private bool _isCopying;

        private double? _totalSize;
        private double _progressValue;
        private double _receivedSize;

        private DateTime _remainingTime;
        private DateTime _startTime;
        private Timer _speedTimer;
        private UpdateFileModel _updateMode;
        #endregion

        #region Constructors
        public MainWindowViewModel(string[] args, Action closeInvoke)
        {
            if (args.Length != 6) return;

            CloseWindowCmd = new RelayCommand(closeInvoke);
            UpdateLogCmd = new RelayCommand(UpdateLogInvoke);
            UpdatePlatform(args);
        }
        #endregion

        #region Public Properties

        public ICommand UpdateLogCmd { get; set; }

        public ICommand CloseWindowCmd { get; set; }

        /// <summary>
        /// 下载模式
        /// </summary>
        public bool IsDownloading
        {
            get { return _isDownloading; }
            set { SetProperty(ref _isDownloading, value); }
        }

        public bool IsCopying
        {
            get { return _isCopying; }
            set { SetProperty(ref _isCopying, value); }
        }

        /// <summary>
        /// 文件大小
        /// </summary>
        public double? TotalSize
        {
            get { return _totalSize; }
            set { SetProperty(ref _totalSize, value); }
        }

        /// <summary>
        /// 当前状态
        /// </summary>
        public string StatusDescription
        {
            get { return _statusDescription; }
            set { SetProperty(ref _statusDescription, value); }
        }

        /// <summary>
        /// 下载剩余时间
        /// </summary>
        public DateTime RemainingTime
        {
            get { return _remainingTime; }
            set { SetProperty(ref _remainingTime, value); }
        }

        /// <summary>
        /// 下载速度
        /// </summary>
        public string DownLoadSpeed
        {
            get { return _downLoadSpeed; }
            set { SetProperty(ref _downLoadSpeed, value); }
        }

        /// <summary>
        /// 已下载文件大小
        /// </summary>
        public double ReceivedSize
        {
            get { return _receivedSize; }
            set { SetProperty(ref _receivedSize, value); }
        }

        /// <summary>
        /// 进度条的值
        /// </summary>
        public double ProgressValue
        {
            get { return _progressValue; }
            set { SetProperty(ref _progressValue, value); }
        }

        /// <summary>
        /// 安装文件的文件名
        /// </summary>
        public string InstallFileName
        {
            get { return _installFileName; }
            set { SetProperty(ref _installFileName, value); }
        }

        public UpdateFileModel UpdateInfo
        {
            get { return _updateMode ?? (_updateMode = new UpdateFileModel()); }
        }

        #endregion

        #region Private Methods

        private void UpdateLogInvoke()
        {
            Process.Start(UpdateInfo.UpdateLogUrl);
        }

        private void UpdatePlatform(string[] args)
        {
            //初始化参数
            UpdateInfo.CurrentVersion = args[0];
            UpdateInfo.NewVersion = args[1];
            UpdateInfo.UpdateLogUrl = args[2];
            UpdateInfo.UpdateFileUrl = args[3];
            UpdateInfo.UnpackPath = args[4].Replace("|", " ");
            UpdateInfo.FileMd5 = args[5];

            //开始下载
            UpdateInfo.TempPath = Utility.GetTempDirectory() + "\\";
            var filePath = UpdateInfo.TempPath + UpdateInfo.FileName;
            StatusDescription = " 正在下载...";
            IsDownloading = true;
            _speedTimer = new Timer(SpeedTimerOnTick, null, 0, 1000);
            _startTime = DateTime.Now;
            DownLoadFile(UpdateInfo.UpdateFileUrl, filePath);
        }

        private void DownLoadFile(string remotePath, string localPath)
        {
            var webClient = new WebClient();
            webClient.DownloadProgressChanged += OnDownloadProgressChanged;
            webClient.DownloadFileCompleted += OnDownloadFileCompleted;
            webClient.DownloadFileAsync(new Uri(remotePath), localPath);
        }

        private void OnDownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            var value = e.ProgressPercentage;
            if (value > 90)
            {
                value = 90;
            }
            ProgressValue = value;

            ReceivedSize = e.BytesReceived / (1024.0 * 1024.0);
            TotalSize = e.TotalBytesToReceive / (1024.0 * 1024.0);
            UpdateInfo.ReceivedBytes = e.BytesReceived;
            UpdateInfo.TotalBytes = e.TotalBytesToReceive;
        }

        private void OnDownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                StatusDescription = "更新被取消！";
                return;
            }

            if (e.Error is WebException)
            {
                StatusDescription = "请检测您的网络或防火墙设置！";
                return;
            }

            //下载完成
            _speedTimer.Dispose();
            _speedTimer = null;
            IsDownloading = false;

            //验证文件
            var filePath = UpdateInfo.TempPath + UpdateInfo.FileName;
            StatusDescription = "安全校验...";
            if (!VerifyFileMd5(filePath)) return;
   
            //解压
            StatusDescription = " 正在解压...";
            ZipFile.ExtractToDirectory(filePath, UpdateInfo.TempPath);
            File.Delete(filePath);
            Update32Or64Libs(UpdateInfo.TempPath);
            ProgressValue = +ProgressValue + 5;

            //更新
            StatusDescription = " 正在更新...";
            IsCopying = true;
            Utility.DirectoryCopy(UpdateInfo.TempPath, UpdateInfo.UnpackPath, 
                true, true, o => InstallFileName = o);
            Utility.UpdateReg(Registry.LocalMachine, SubKey, "DisplayVersion", 
                UpdateInfo.NewVersion);
            ExecuteStrategy();
            IsCopying = false;
            ProgressValue = +ProgressValue + 5;

            //启动平台
            StatusDescription = " 启动平台...";
            Directory.Delete(UpdateInfo.TempPath, true);
            Loger.Print(string.Format("update version {0} to {1} succeeded. ", 
                UpdateInfo.CurrentVersion, UpdateInfo.NewVersion));
            Thread.Sleep(500);
            Process.Start(UpdateInfo.UnpackPath + "/Platform.exe");
            Application.Current.Dispatcher.Invoke(() => CloseWindowCmd.Execute(null));
        }

        private bool VerifyFileMd5(string fileName)
        {
            Thread.Sleep(1000);
            var md5 = Utility.GetFileMD5(fileName);
            if (!UpdateInfo.FileMd5.Equals(md5))
            {
                StatusDescription = "更新失败，更新文件MD5码不一致！";
                Loger.Print("Update file MD5 inconsistent. ");
                Directory.Delete(UpdateInfo.TempPath, true); ;
                return false;
            }

            return true;
        }

        private void SpeedTimerOnTick(object sender)
        {
            var interval = DateTime.Now - _startTime;

            //下载速度
            DownLoadSpeed = interval.Seconds < 1
                ? Utility.ConvertToUnit(UpdateInfo.ReceivedBytes)
                : Utility.ConvertToUnit(UpdateInfo.ReceivedBytes / interval.Seconds);

            //剩余时间
            var size = (UpdateInfo.TotalBytes - UpdateInfo.ReceivedBytes) / (1024 * 1024);
            RemainingTime = new DateTime().AddSeconds(Convert.ToDouble(size));
        }

        private void Update32Or64Libs(string currentDir)
        {
            var is64XSystem = Environment.Is64BitOperatingSystem;
            var sourceDir = Path.Combine(currentDir, is64XSystem ? "x64" : "x32");
            var destDir = Path.Combine(currentDir, "dlls");

            if (!Directory.Exists(sourceDir)) return;
            Utility.DirectoryCopy(sourceDir, destDir, true, true, null);
            Directory.Delete(sourceDir);
        }

        private void ExecuteStrategy()
        {
            var dllPath = UpdateInfo.UnpackPath + "UpdaterEx.dll";
            if (!File.Exists(dllPath)) return;
            var dll = Assembly.LoadFile(dllPath);
            foreach (var type in dll.GetExportedTypes())
            {
                dynamic dy = Activator.CreateInstance(type);
                dy.Execute(UpdateInfo.CurrentVersion, UpdateInfo.NewVersion);
            }
        }
        #endregion
    }
}
