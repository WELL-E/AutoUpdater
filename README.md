# GeneralUpdate #
![](https://img.shields.io/github/license/WELL-E/AutoUpdater?color=blue)
![](https://img.shields.io/github/stars/WELL-E/AutoUpdater?color=blue)
![](https://img.shields.io/github/stars/WELL-E/AutoUpdater/workflows/dotnet-ci/badge.svg?color=blue)


![](imgs/GeneralUpdate_h.png)

[English introduction](https://github.com/WELL-E/AutoUpdater/blob/master/README_en.md)

### 组件介绍 ###

GeneralUpdate是基于.net standard开发的一款（c/s应用）自动升级程序。该组件将更新的核心部分抽离出来方便应用于多种项目当中目前适用于wpf，控制台应用，winfrom。

### 支持框架

理论支撑：https://docs.microsoft.com/zh-cn/dotnet/standard/net-standard

| 框架名称                              | 是否支持         |
| ------------------------------------- | ---------------- |
| .NET Core 2.0                         | 支持             |
| .NET 5 6 7                            | 支持             |
| .NET Framework 4.6.1                  | 支持             |
| Mono 5.4                              | 理论支持，未验证 |
| Xamarin.iOS                           | 理论支持，未验证 |
| Xamarin.Mac                           | 理论支持，未验证 |
| Xamarin.Android                       | 理论支持，未验证 |
| Universal Windows Platform 10.0.16299 | 理论支持，未验证 |
| Unity 2018.1                          | 理论支持，未验证 |

| UI框架名称        | 是否支持                      |
| ----------------- | ----------------------------- |
| WPF               | 支持                          |
| UWP               | 商店模式下不可更新（lindexi） |
| MAUI              | 暂不支持，正在兼容            |
| Avalonia          | 未验证，等待反馈              |
| WinUI             | 未验证，等待反馈              |
| Console（控制台） | 支持                          |

### 操作系统

| 操作系统名称 | 是否支持 |
| ------------ | -------- |
| Windows      | 支持     |
| Linux        | 未验证   |
| Mac          | 未验证   |
| iOS          | 暂不支持 |
| Android      | 暂不支持 |

### 功能介绍 ###

- GeneralUpdate.Core：断点续传、逐版本更新。
- GeneralUpdate.ClientCore：断点续传、逐版本更新、更新组件自更新、便捷启动更新组件
- GeneralUpdate.AspNetCore：服务端支持更新包下载地址、版本信息等内容。
- GeneralUpdate.Zip：解压更新包、解压进度通知。
- GeneralUpdate.Single：应用程序单例运行。
- GeneralUpdate.Common：组件公共类、方法。
- 源码"sql"目录下包含mysql数据库表内容的生成脚本。

### 帮助文档 ###
- 讲解视频： https://www.bilibili.com/video/BV1aX4y137dd
- 官方网站： http://justerzhu.cn/

### 讨论组 ###
GeneralUpdate开源项目讨论QQ群：748744489

.Net技术讨论QQ群：580749909

### 开源地址 ###
- https://github.com/WELL-E/AutoUpdater
- https://gitee.com/Juster-zhu/GeneralUpdate

### 更新流程图 ###
![](imgs/flow2.png)

### 运行效果及更新流程展示 ###
![](imgs/run.jpg)

## 支持作者 ###

作者：Juster.zhu & Charles.Yu

![](imgs/money.jpg)