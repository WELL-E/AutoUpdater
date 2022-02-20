# GeneralUpdate #
![](https://img.shields.io/github/license/WELL-E/AutoUpdater?color=blue)
![](https://img.shields.io/github/stars/WELL-E/AutoUpdater?color=blue)

![](imgs/GeneralUpdate_h.png)

### Component is introduced ###

GeneralUpdate is an automatic update program (c/s application) developed based on .net standard. This component extracts the core part of the update for easy application in a variety of projects. Currently, it is suitable for wpf, console applications, and winfrom. 

### Support frame 

theoretical support ：https://docs.microsoft.com/zh-cn/dotnet/standard/net-standard

| frame name                            | support                           |
| ------------------------------------- | --------------------------------- |
| .NET Core 2.0                         | yes                               |
| .NET 5 6 7                            | yes                               |
| .NET Framework 4.6.1                  | yes                               |
| Mono 5.4                              | theoretical support, not verified |
| Xamarin.iOS                           | theoretical support, not verified |
| Xamarin.Mac                           | theoretical support, not verified |
| Xamarin.Android                       | theoretical support, not verified |
| Universal Windows Platform 10.0.16299 | theoretical support, not verified |
| Unity 2018.1                          | theoretical support, not verified |

| UI frame name | support                             |
| ------------- | ----------------------------------- |
| WPF           | yes                                 |
| UWP           | theoretical support, not verified   |
| MAUI          | Not currently supported, compatible |
| Avalonia      | theoretical support, not verified   |
| WinUI         | theoretical support, not verified   |
| Console       | yes                                 |

### System

| System name | support |
| ----------- | ------- |
| Windows     | yes     |
| Linux       | unknow  |
| Mac         | unknow  |
| iOS         | no      |
| Android     | no      |

### Function is introduced ###

- GeneralUpdate.Core：Breakpoint continuation, version by version update.
- GeneralUpdate.ClientCore：Breakpoint continuation, version-by-version update, update component self-update, easy to start update components.
- GeneralUpdate.AspNetCore：The server supports updating the download address and version of the package.
- GeneralUpdate.Zip：Decompress the update package and progress notification.
- GeneralUpdate.Single：Application singletons run.
- GeneralUpdate.Common：Component public classes, methods.
- Source "SQL" directory contains mysql database table content generation script.

### Help document ###
- Interpretation of the video： https://www.bilibili.com/video/BV1aX4y137dd
- The official website： http://justerzhu.cn/

### Discussion groups ###
GeneralUpdate QQ Group：748744489

### Open source address ###
- https://github.com/WELL-E/AutoUpdater
- https://gitee.com/Juster-zhu/GeneralUpdate

### Update flow chart ###
![](imgs/flow2.png)

### Operation effect and update process display ###
![](imgs/run.jpg)

## Author ###

Juster.zhu & Charles.Yu
