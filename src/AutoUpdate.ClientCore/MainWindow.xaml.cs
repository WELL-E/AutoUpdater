using GeneralUpdate.ClientCore;
using GeneralUpdate.ClientCore.Strategys;
using GeneralUpdate.Common.Models;
using GeneralUpdate.Core.Update;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using ExceptionEventArgs = GeneralUpdate.Core.Update.ExceptionEventArgs;

namespace AutoUpdate.ClientCore
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string baseUrl = @"http://127.0.0.1:5001";

        public MainWindow()
        {
            InitializeComponent();
        }


        #region GeneralUpdate Core

        private ClientParameter clientParameter;

        private void BtnClientTest_Click(object sender, RoutedEventArgs e)
        {
            Task.Run(async () =>
            {
                //主程序信息
                var mainVersion = "1.1.1";
                var mianType = 1;

                //该对象用于主程序客户端与更新组件进程之间交互用的对象
                clientParameter = new ClientParameter();
                //更新组件的版本号
                clientParameter.ClientVersion = "1.1.1";
                //客户端类型：1.主程序客户端 2.更新组件
                clientParameter.ClientType = 2;
                //更新程序exe名称
                clientParameter.AppName = "AutoUpdate.ConsoleApp";
                //主程序客户端exe名称
                clientParameter.MainAppName = "AutoUpdate.Test";
                //本机的客户端程序应用地址
                clientParameter.InstallPath = @"D:\Updatetest_hub\Run_app";
                //更新公告网页
                //clientParameter.UpdateLogUrl = "https://www.baidu.com/";
                //更新组件请求验证更新的服务端地址
                clientParameter.ValidateUrl = $"{baseUrl}/validate/{ clientParameter.ClientType }/{ clientParameter.ClientVersion }";
                //更新组件更新包下载地址
                clientParameter.UpdateUrl = $"{baseUrl}/versions/{ clientParameter.ClientType }/{ clientParameter.ClientVersion }";
                //主程序客户端请求验证更新的服务端地址
                clientParameter.MainValidateUrl = $"{baseUrl}/validate/{ mianType }/{ mainVersion }";
                //主程序客户端更新包下载地址
                clientParameter.MainUpdateUrl = $"{baseUrl}/versions/{ mianType }/{ mainVersion }";

               var  generalClientBootstrap = new GeneralClientBootstrap();
                //单个或多个更新包下载通知事件
                generalClientBootstrap.MutiDownloadProgressChanged += OnMutiDownloadProgressChanged;
                //单个或多个更新包下载速度、剩余下载事件、当前下载版本信息通知事件
                generalClientBootstrap.MutiDownloadStatistics += OnMutiDownloadStatistics;
                //单个或多个更新包下载完成
                generalClientBootstrap.MutiDownloadCompleted += OnMutiDownloadCompleted;
                //完成所有的下载任务通知
                generalClientBootstrap.MutiAllDownloadCompleted += OnMutiAllDownloadCompleted;
                //下载过程出现的异常通知
                generalClientBootstrap.MutiDownloadError += OnMutiDownloadError;
                //整个更新过程出现的任何问题都会通过这个事件通知
                generalClientBootstrap.Exception += OnException;
                //ClientStrategy该更新策略将完成1.自动升级组件自更新 2.启动更新组件 3.配置好ClientParameter无需再像之前的版本写args数组进程通讯了。
                generalClientBootstrap.Config(clientParameter).
                Option(UpdateOption.CompressEncoding,System.Text.Encoding.UTF8).
                Option(UpdateOption.CompressFormat,"zip").
                Strategy<ClientStrategy>();
                await generalClientBootstrap.LaunchAsync();
            });
        }

        private void OnMutiDownloadStatistics(object sender, MutiDownloadStatisticsEventArgs e)
        {
            //e.Remaining 剩余下载时间
            //e.Speed 下载速度
            //e.Version 当前下载的版本信息
        }

        private void OnMutiDownloadProgressChanged(object sender, MutiDownloadProgressChangedEventArgs e)
        {
            //e.TotalBytesToReceive 当前更新包需要下载的总大小
            //e.ProgressValue 当前进度值
            //e.ProgressPercentage 当前进度的百分比
            //e.Version 当前下载的版本信息
            //e.Type 当前正在执行的操作  1.ProgressType.Check 检查版本信息中 2.ProgressType.Donwload 正在下载当前版本 3. ProgressType.Updatefile 更新当前版本 4. ProgressType.Done更新完成 5.ProgressType.Fail 更新失败
            //e.BytesReceived 已下载大小
        }

        private void OnException(object sender, ExceptionEventArgs e)
        {
            Debug.WriteLine(e.Exception.Message);
        }

        private void OnMutiAllDownloadCompleted(object sender, MutiAllDownloadCompletedEventArgs e)
        {
            //e.FailedVersions; 如果出现下载失败则会把下载错误的版本、错误原因统计到该集合当中。
            Debug.WriteLine($"Is all download completed { e.IsAllDownloadCompleted }.");
        }

        private void OnMutiDownloadCompleted(object sender, MutiDownloadCompletedEventArgs e)
        {
            //Debug.WriteLine($"{ e.Version.Name } download completed.");
        }

        private void OnMutiDownloadError(object sender, MutiDownloadErrorEventArgs e)
        {
            //Debug.WriteLine($"{ e.Version.Name } error!");
        }

        #endregion
    }
}
