# GeneralUpdate #

GeneralUpdate是基于.net framwork4.5.2开发的一款（c/s应用）自动升级程序。

第一个版本叫Autoupdate（原博客： [WPF自动更新程序](https://www.cnblogs.com/igotogo/p/6899921.html)）有人会奇怪为什么会改名称，稍微解释一下是因为在nuget上有重名的项目再者就是新版本更新功能不仅限于wpf程序的更新。

将更新的核心部分抽离出来方便应用于多种项目当中目前适用于wpf，控制台应用，winfrom。相比以前更方便的是不需要在过分关注源码可直接通过nuget直接使用。

如果有任何使用问题可以在Github的issues上进行提问我会每周统一找时间解决并解答bug或者问题。或者联系文章末尾的联系方式会有人解答。


## 如何使用： ##
Gitee（码云）地址：https://gitee.com/Juster-zhu/GeneralUpdate

Nuget地址：https://www.nuget.org/packages/GeneralUpdate.Core/

GitHub地址： https://github.com/WELL-E/AutoUpdater GeneralUpdate版本在 Branch：Autoupdate2。

issues：https://github.com/WELL-E/AutoUpdater/issues

![](https://img2020.cnblogs.com/blog/1214710/202005/1214710-20200503171621516-1381696009.png)

运行截图：
![](https://img2020.cnblogs.com/blog/1214710/202005/1214710-20200503171309275-1921529733.png)


快速启动：

![](https://img2020.cnblogs.com/blog/1214710/202005/1214710-20200517182549723-1185859724.png)


    #region Launch1

            args = new string[6] {
                "0.0.0.0",
                "1.1.1.1",
                "https://github.com/WELL-E",
                 "http://192.168.50.225:7000/update.zip",
                 @"E:\PlatformPath",
                "509f0ede227de4a662763a4abe3d8470",
                 };

            GeneralUpdateBootstrap bootstrap = new GeneralUpdateBootstrap();//自动更新引导类
            bootstrap.DownloadStatistics += OnDownloadStatistics;//下载进度通知事件
            bootstrap.ProgressChanged += OnProgressChanged;//更新进度通知事件
            bootstrap.Strategy<DefultStrategy>().//注册策略，可自定义更新流程
                Option(UpdateOption.Format, "zip").//指定更新包的格式，目前只支持zip
                Option(UpdateOption.MainApp, "your application name").//指定更新完成后需要启动的主程序名称不需要加.exe直接写名称即可
                RemoteAddress(args).//这里的参数保留了之前的参数数组集合
                Launch();//启动更新

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
            //    Option(UpdateOption.MainApp, "").
            //    RemoteAddress(@"https://api.com/GeneralUpdate?version=1.0.0.1").//指定更新地址
            //    Launch();

            #endregion

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

关于调试：

调试本程序如果没有服务端支持可用hfs这款软件，作为临时服务器进行调试使用方式非常简单下载好了之后界面如下

![](https://img2020.cnblogs.com/blog/1214710/202005/1214710-20200517175400462-1015101986.png)
![](https://img2020.cnblogs.com/blog/1214710/202005/1214710-20200517175525111-190800663.png)

 开发作者： JusterZhu & WELL-E

联系我们：
QQ群：

WELL-E

- 1群 130108655
- 2群 960655709
- Git: https://github.com/WELL-E

juster.chu

- E-Mail：zhuzhen723723@outlook.com
- QQ: 580749909(个人群)
- Blog： https://www.cnblogs.com/justzhuzhu/
- Git: https://github.com/JusterZhu
- 微信公众号

![](https://img2020.cnblogs.com/i-beta/1214710/202003/1214710-20200302173106033-1322582358.png)