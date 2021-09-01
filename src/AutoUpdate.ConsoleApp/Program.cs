using GeneralUpdate.Common.Models;
using GeneralUpdate.Core;
using GeneralUpdate.Core.Strategys;
using GeneralUpdate.Core.Update;
using Newtonsoft.Json;
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
            var resultBase64 = args[0];
            var bootstrap = new GeneralUpdateBootstrap();
            bootstrap.Exception += OnException;
            bootstrap.MutiDownloadError += OnMutiDownloadError;
            bootstrap.MutiDownloadCompleted += OnMutiDownloadCompleted;
            bootstrap.MutiDownloadStatistics += OnMutiDownloadStatistics;
            bootstrap.MutiDownloadProgressChanged += OnMutiDownloadProgressChanged;
            bootstrap.MutiAllDownloadCompleted += OnMutiAllDownloadCompleted;
            bootstrap.Strategy<DefaultStrategy>().
                //下载超时时间（单位：秒）,如果不指定则默认超时时间为30秒。
                Option(UpdateOption.DownloadTimeOut, 60).
                RemoteAddressBase64(resultBase64).
                LaunchAsync();

            Console.Read();
        }

        private static void OnMutiDownloadStatistics(object sender, MutiDownloadStatisticsEventArgs e)
        {
            Console.WriteLine($"{ e.Remaining },{ e.Speed },{ e.Version.Name }.");
        }

        private static void OnException(object sender, ExceptionEventArgs e)
        {
            Console.WriteLine(e.Exception.Message);
        }

        private static void OnMutiDownloadProgressChanged(object sender, MutiDownloadProgressChangedEventArgs e)
        {
            switch (e.Type)
            {
                case ProgressType.Check:
                    Console.WriteLine($"{ e.Message }");
                    break;
                case ProgressType.Donwload:
                    Console.WriteLine($"{ e.Version.Name },{ e.ProgressValue },{ e.ProgressPercentage },{ e.TotalBytesToReceive }");
                    break;
                case ProgressType.Updatefile:
                    Console.WriteLine($"{ e.Message }");
                    break;
                case ProgressType.Done:
                    Console.WriteLine($"{ e.Message }");
                    break;
                case ProgressType.Fail:
                    Console.WriteLine($"{ e.Message }");
                    break;
            }
        }

        private static void OnMutiDownloadError(object sender, MutiDownloadErrorEventArgs e)
        {
            Console.WriteLine($"{ e.Version.Name } ,{ e.Exception.Message }");
        }

        private static void OnMutiDownloadCompleted(object sender, MutiDownloadCompletedEventArgs e)
        {
            Console.WriteLine($"{ e.Version.Name },{ e.Version.Version },{ e.Version.Url }.");
        }

        private static void OnMutiAllDownloadCompleted(object sender, MutiAllDownloadCompletedEventArgs e)
        {
            //e.FailedVersions; 如果出现下载失败则会把下载错误的版本、错误原因统计到该集合当中。

            //是否下载成功
            if (e.IsAllDownloadCompleted)
            {
                Console.WriteLine("All download completed.");
            }
            else
            {
                Console.WriteLine("download failed!");
            }
        }
    }
}
