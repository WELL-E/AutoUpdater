using AutoUpdate.WpfApp.Common;
using GeneralUpdate.Core;
using GeneralUpdate.Core.Strategys;
using GeneralUpdate.Core.Update;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AutoUpdate.WpfApp.ViewModels
{
    public class MainViewModel
    {
        public ICommand CloseWindowCmd { get; set; }

        public MainViewModel(string[] args, Action closeInvoke)
        {
            if (args.Length != 6) return;

            CloseWindowCmd = new RelayCommand(closeInvoke);
            GeneralUpdateBootstrap bootstrap = new GeneralUpdateBootstrap();
            bootstrap.DownloadStatistics += Bootstrap_DownloadStatistics;
            bootstrap.ProgressChanged += Bootstrap_ProgressChanged;
            bootstrap.Strategy<DefultStrategy>().
                //指定更新包的格式，目前只支持zip。不指定则默认为zip。
                Option(UpdateOption.Format, "zip").
                //指定更新完成后需要启动的主程序名称不需要加.exe直接写名称即可
                Option(UpdateOption.MainApp, "your application name").
                //下载超时时间（单位：秒）,如果不指定则默认超时时间为30秒。
                Option(UpdateOption.DownloadTimeOut, 60).
                //这里的参数保留了之前的参数数组集合
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
