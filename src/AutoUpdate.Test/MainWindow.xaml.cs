using GeneralUpdate.ClientCore;
using GeneralUpdate.ClientCore.Strategys;
using GeneralUpdate.Common.Models;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AutoUpdate.Test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ClientParameter clientParameter;
        private GeneralClientBootstrap generalClientBootstrap;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Task.Run(async()=> 
            {
                //Clinet version.
                var mainVersion = "1.1.1";
                var mianType = 1;

                //Updater version
                clientParameter = new ClientParameter();
                clientParameter.ClientVersion = "1.1.1";
                clientParameter.ClientType = 2;
                clientParameter.AppName = "AutoUpdate.ConsoleApp";
                clientParameter.MainAppName = "AutoUpdate.Test";
                clientParameter.InstallPath = @"D:\update_test";
                clientParameter.UpdateLogUrl = "https://www.baidu.com/";
                clientParameter.ValidateUrl = $"https://127.0.0.1:5001/api/update/getUpdateValidate/{ clientParameter.ClientType }/{ clientParameter.ClientVersion }";
                clientParameter.UpdateUrl = $"https://127.0.0.1:5001/api/update/getUpdateVersions/{ clientParameter.ClientType }/{ clientParameter.ClientVersion }";
                clientParameter.MainValidateUrl = $"https://127.0.0.1:5001/api/update/getUpdateValidate/{ mianType }/{ mainVersion }";
                clientParameter.MainUpdateUrl = $"https://127.0.0.1:5001/api/update/getUpdateVersions/{ mianType }/{ mainVersion }";

                generalClientBootstrap = new GeneralClientBootstrap();
                generalClientBootstrap.MutiDownloadProgressChanged += OnMutiDownloadProgressChanged;
                generalClientBootstrap.MutiDownloadStatistics += OnMutiDownloadStatistics;
                generalClientBootstrap.MutiDownloadCompleted += OnMutiDownloadCompleted;
                generalClientBootstrap.MutiAllDownloadCompleted += OnMutiAllDownloadCompleted;
                generalClientBootstrap.MutiDownloadError += OnMutiDownloadError;
                generalClientBootstrap.Exception += OnException;
                generalClientBootstrap.Config(clientParameter).
                    Strategy<ClientStrategy>();
                await generalClientBootstrap.LaunchTaskAsync();
            });
        }

        private void OnMutiDownloadStatistics(object sender, GeneralUpdate.Core.Update.MutiDownloadStatisticsEventArgs e)
        {
            //Debug.WriteLine($"{ e.Version.Name } ,{ e.Remaining },{ e.Speed }");
        }

        private void OnException(object sender, GeneralUpdate.Core.Update.ExceptionEventArgs e)
        {
            Debug.WriteLine(e.Exception.Message);
        }

        private void OnMutiAllDownloadCompleted(object sender, GeneralUpdate.Core.Update.MutiAllDownloadCompletedEventArgs e)
        {
            Debug.WriteLine($"Is all download completed { e.IsAllDownloadCompleted }.");
        }

        private void OnMutiDownloadCompleted(object sender, GeneralUpdate.Core.Update.MutiDownloadCompletedEventArgs e)
        {
            Debug.WriteLine($"{ e.Version.Name } download completed.");
        }

        private void OnMutiDownloadError(object sender, GeneralUpdate.Core.Update.MutiDownloadErrorEventArgs e)
        {
            Debug.WriteLine($"{ e.Version.Name } error!");
        }

        private void OnMutiDownloadProgressChanged(object sender, GeneralUpdate.Core.Update.MutiDownloadProgressChangedEventArgs e)
        {
            //var name = e.Version == null ? "" : e.Version.Name;
            //Debug.WriteLine($"{ name }, ProgressValue - { e.ProgressValue }, ProgressPercentage - { e.ProgressPercentage }.");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            bool? isOpen = openFile.ShowDialog(this);
            if (isOpen.Value)
            {
               var name = openFile.FileName;
               var md5 = GetFileMD5(name);
               TxtMD5.Text= md5;
            }
        }

        internal string GetFileMD5(string fileName)
        {
            try
            {
                var file = new FileStream(fileName, FileMode.Open);
                var md5 = new MD5CryptoServiceProvider();
                var retVal = md5.ComputeHash(file);
                file.Close();
                var sb = new StringBuilder();
                for (var i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                return sb.ToString();
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
}
