GeneralUpdate


---------------

### 简介:

一款基于.net开发的c/s架构自动更新程序。wpf winfrom console均能更新。
更新不仅限于wpf程序的更新讲更新的核心部分抽离出来方便大家使用并应用于多种项目当中目前适用于wpf，
控制台应用，winfrom。相比以前更方便的是不需要在过分关注源码可直接通过nuget直接使用。

github

----------

Nuget地址：https://www.nuget.org/packages/GeneralUpdate.Core/

GitHub地址： https://github.com/WELL-E/AutoUpdater GeneralUpdate版本在 Branch：Autoupdate2。

issues：https://github.com/WELL-E/AutoUpdater/issues

开发作者： [JusterZhu](https://github.com/JusterZhu)  &  [WELL-E](https://github.com/WELL-E)

### 使用方式:

    Launch1
           string args = new string[6] {
                "0.0.0.0",
                "1.1.1.1",
                "https://github.com/WELL-E",
                 "http://192.168.225.225:7000/update.zip",
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
	     
    Launch2

        GeneralUpdateBootstrap bootstrap2 = new GeneralUpdateBootstrap();
        bootstrap2.DownloadStatistics += OnDownloadStatistics;
        bootstrap2.ProgressChanged += OnProgressChanged;
        bootstrap2.Strategy<DefultStrategy>().
            Option(UpdateOption.Format, "zip").
            Option(UpdateOption.MainApp, "KGS.CPP").
            RemoteAddress(@"https://api.com/GeneralUpdate?version=1.0.0.1").
            Launch();


       private void OnProgressChanged(object sender, ProgressChangedEventArgs e)
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

        private void OnDownloadStatistics(object sender, DownloadStatisticsEventArgs e)
        {
            Console.WriteLine($"下载速度：{e.Speed}，剩余时间：{e.Remaining.Minute}:{e.Remaining.Second}");
        }


### 开源协议:

Open sourced under the MIT license.

### 联系我们：

QQ群： 

- 	WELL-E
	- 	1群 130108655
	- 	2群 960655709
	- 	Git: https://github.com/WELL-E


- 	juster.chu
	- 	E-Mail：zhuzhen723723@outlook.com
	- 	QQ: 580749909(个人群)
	- 	Blog： https://www.cnblogs.com/justzhuzhu/
	- 	Git: https://github.com/JusterZhu



