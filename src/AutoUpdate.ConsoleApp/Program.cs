using GeneralUpdate.Core;
using GeneralUpdate.Core.Strategys;
using GeneralUpdate.Core.Update;
using GeneralUpdate.Core.Utils;
using System;

namespace AutoApdate.ConsoleApp
{
    class Program
    {
        /// <summary>
        /// Quick start
        /// 本程序中引用的第三方组件均来自nuget包并遵循MIT开源协议 https://spdx.org/licenses/MIT.html
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            #region Launch1

            args = new string[6] {
                "0.0.0.0",
                "1.1.1.1",
                "https://github.com/WELL-E",
                 "http://192.168.50.225:7000/update.zip",
                 @"E:\PlatformPath",
                "5086d584dd81360a15d84d863adadfb3",
                 };

            GeneralUpdateBootstrap bootstrap = new GeneralUpdateBootstrap();
            bootstrap.DownloadStatistics += OnDownloadStatistics;
            bootstrap.ProgressChanged += OnProgressChanged;
            bootstrap.Strategy<DefultStrategy>().
                //指定更新包的格式，目前只支持zip。不指定则默认为zip。
                Option(UpdateOption.Format, "zip").
                //指定更新完成后需要启动的主程序名称不需要加.exe直接写名称即可
                Option(UpdateOption.MainApp, "your application name").
                //下载超时时间（单位：秒）,如果不指定则默认超时时间为30秒。
                Option(UpdateOption.DownloadTimeOut,60).
                //这里的参数保留了之前的参数数组集合
                RemoteAddress(args).
                Launch();

            #endregion

            #region Launch2

            /*
             * Launch2
             * 新增了第二种启动方式
             * 流程：
             * 1.指定更新地址，https://api.com/GeneralUpdate?version=1.0.0.1 在webapi中传入客户端当前版本号
             * 2.如果需要更新api回返回给你所有的更新信息（详情内容参考 /Models/UpdateInfo.cs）
             * 3.拿到更新信息之后则开始http请求更新包
             * 4.下载
             * 5.解压
             * 6.更新本地文件
             * 7.关闭更新程序
             * 8.启动配置好主程序
             * 更新程序必须跟主程序放在同级目录下
             */

            //GeneralUpdateBootstrap bootstrap2 = new GeneralUpdateBootstrap();
            //bootstrap2.DownloadStatistics += OnDownloadStatistics;
            //bootstrap2.ProgressChanged += OnProgressChanged;
            //bootstrap2.Strategy<DefultStrategy>().
            //    Option(UpdateOption.Format, "zip").
            //    Option(UpdateOption.MainApp, "your application name").
            //    RemoteAddress(@"https://api.com/GeneralUpdate?version=1.0.0.1").//指定更新地址
            //    Option(UpdateOption.DownloadTimeOut, 60).
            //    Launch();

            #endregion

            Console.Read();
        }

        private static void OnProgressChanged(object sender, ProgressChangedEventArgs e)
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

        private static void OnDownloadStatistics(object sender, DownloadStatisticsEventArgs e)
        {
            Console.WriteLine($"下载速度：{e.Speed}，剩余时间：{e.Remaining.Minute}:{e.Remaining.Second}");
        }
    }
}
