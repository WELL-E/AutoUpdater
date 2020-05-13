using GeneralUpdate.Core;
using GeneralUpdate.Core.Strategys;
using GeneralUpdate.Core.Update;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoUpdate.WpfApp.ViewModels
{
    public class MainViewModel
    {
        public MainViewModel() {
            Start();
        }

        public void Start() {

           var args = new string[6] {
                "0.0.0.0",
                "1.1.1.1",
                "https://github.com/WELL-E",
                 "http://192.168.50.225:7000/update.zip",
                 @"E:\PlatformPath",
                "509f0ede227de4a662763a4abe3d8470",
                 };

            GeneralUpdateBootstrap bootstrap = new GeneralUpdateBootstrap();
            bootstrap.DownloadStatistics += Bootstrap_DownloadStatistics; ;
            bootstrap.ProgressChanged += Bootstrap_ProgressChanged; ;
            bootstrap.Strategy<DefultStrategy>().
                Option(UpdateOption.Format, "zip").
                Option(UpdateOption.MainApp, "your application name").
                RemoteAddress(args).
                Launch();

        }

        private void Bootstrap_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.Type == ProgressType.Updatefile)
            {
                var str = $"当前更新第：{e.ProgressValue}个,更新文件总数：{e.TotalSize}";
                Console.WriteLine(str);
            }

            if (e.Type == ProgressType.Done)
            {
                Console.WriteLine("更新完成");
            }
        }

        private void Bootstrap_DownloadStatistics(object sender, DownloadStatisticsEventArgs e)
        {
            Console.WriteLine($"下载速度：{e.Speed}，剩余时间：{e.Remaining.Minute}:{e.Remaining.Second}");
        }
    }
}
