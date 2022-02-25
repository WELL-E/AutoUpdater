using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AutoUpdate.Core
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        #region GeneralUpdate Client Core

        private void BtnClientTest_Click(object sender, RoutedEventArgs e)
        {
            Task.Run(async () =>
            {
                GeneralClientBootstrap generalClientBootstrap;
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
                clientParameter.AppName = "AutoUpdate.WpfNet6-Sample";
                //主程序客户端exe名称
                clientParameter.MainAppName = "AutoUpdate.Test";
                //本机的客户端程序应用地址
                clientParameter.InstallPath = @"D:\Updatetest_hub\Run_app";
                //更新公告网页
                clientParameter.UpdateLogUrl = "https://www.baidu.com/";
                //更新组件请求验证更新的服务端地址
                clientParameter.ValidateUrl = $"http://127.0.0.1:5001/validate/{ clientParameter.ClientType }/{ clientParameter.ClientVersion }";
                //更新组件更新包下载地址
                clientParameter.UpdateUrl = $"http://127.0.0.1:5001/versions/{ clientParameter.ClientType }/{ clientParameter.ClientVersion }";
                //主程序客户端请求验证更新的服务端地址
                clientParameter.MainValidateUrl = $"http://127.0.0.1:5001/validate/{ mianType }/{ mainVersion }";
                //主程序客户端更新包下载地址
                clientParameter.MainUpdateUrl = $"http://127.0.0.1:5001/versions/{ mianType }/{ mainVersion }";

                generalClientBootstrap = new GeneralClientBootstrap();
                generalClientBootstrap.Option(UpdateOption.Format, "ZIP");
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
                //ClientParameter
                generalClientBootstrap.Config(clientParameter).
                //ClientStrategy该更新策略将完成1.自动升级组件自更新 2.启动更新组件 3.配置好ClientParameter无需再像之前的版本写args数组进程通讯了。
                    Strategy<ClientStrategy>();
                await generalClientBootstrap.LaunchTaskAsync();
            });
        }

        #endregion
    }
}
