# GeneralUpdate #
![](https://img.shields.io/badge/GeneralUpdate.Core-3.2.1-green)
![](https://img.shields.io/badge/GeneralUpdate.Single-1.0.0-green)
![](https://img.shields.io/badge/GeneralUpdate.Zip-1.0.0-green)
![](https://img.shields.io/github/license/WELL-E/AutoUpdater?color=blue)
![](https://img.shields.io/github/stars/WELL-E/AutoUpdater?color=blue)

![](imgs/GeneralUpdate.png)

### 组件介绍 ###
GeneralUpdate是基于.net standard开发的一款（c/s应用）自动升级程序。该组件将更新的核心部分抽离出来方便应用于多种项目当中目前适用于wpf，控制台应用，winfrom。

### 功能介绍 ###
- GeneralUpdate.Core：断点续传、逐版本更新。
- GeneralUpdate.ClientCore：断点续传、逐版本更新、更新组件自更新、便捷启动更新组件
- GeneralUpdate.AspNetCore：服务端支持更新包下载地址、版本信息等内容。
- GeneralUpdate.Zip：解压更新包、解压进度通知。
- GeneralUpdate.Single：应用程序单例运行。
- GeneralUpdate.Common：组件公共类、方法。

### 帮助文档 ###
- https://www.bilibili.com/video/BV1aX4y137dd
- （官方网站正在开发中）

### 开源地址 ###
- https://github.com/WELL-E/AutoUpdater/tree/autoupdate2
- https://gitee.com/Juster-zhu/GeneralUpdate

### Nuget地址 ###

- https://www.nuget.org/packages/GeneralUpdate.Common/
- https://www.nuget.org/packages/GeneralUpdate.ClientCore/
- https://www.nuget.org/packages/GeneralUpdate.AspNetCore/
- https://www.nuget.org/packages/GeneralUpdate.Zip/
- https://www.nuget.org/packages/GeneralUpdate.Single/
- https://www.nuget.org/packages/GeneralUpdate.Core/

### 运行效果及更新流程展示 ###
![](imgs/run.jpg)

### 快速启动 ###

（1） Example GeneralUpdate.Core

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
            Option(UpdateOption.DownloadTimeOut, 60).
            RemoteAddressBase64(resultBase64).
            LaunchAsync();
    }

（2） Example GeneralUpdate.ClientCore

            //Clinet version.
            var mainVersion = "1.1.1";
            var mianType = 1;

            //Updater version
            clientParameter = new ClientParameter();
            clientParameter.ClientVersion = "1.1.1";
            clientParameter.ClientType = 2;
            clientParameter.AppName = "AutoUpdate.ConsoleApp";
            clientParameter.MainAppName = "AutoUpdate.Test";
            clientParameter.InstallPath = @"D:\update_test";
            clientParameter.UpdateLogUrl = "https://www.baidu.com/";
            clientParameter.ValidateUrl = $"https://127.0.0.1:5001/api/update/getUpdateValidate/{ clientParameter.ClientType }/{ clientParameter.ClientVersion }";
            clientParameter.UpdateUrl = $"https://127.0.0.1:5001/api/update/getUpdateVersions/{ clientParameter.ClientType }/{ clientParameter.ClientVersion }";
            clientParameter.MainValidateUrl = $"https://127.0.0.1:5001/api/update/getUpdateValidate/{ mianType }/{ mainVersion }";
            clientParameter.MainUpdateUrl = $"https://127.0.0.1:5001/api/update/getUpdateVersions/{ mianType }/{ mainVersion }";

            generalClientBootstrap = new GeneralClientBootstrap();
            generalClientBootstrap.MutiDownloadProgressChanged += OnMutiDownloadProgressChanged;
            generalClientBootstrap.MutiDownloadStatistics += OnMutiDownloadStatistics;
            generalClientBootstrap.MutiDownloadCompleted += OnMutiDownloadCompleted;
            generalClientBootstrap.MutiAllDownloadCompleted += OnMutiAllDownloadCompleted;
            generalClientBootstrap.MutiDownloadError += OnMutiDownloadError;
            generalClientBootstrap.Exception += OnException;
            generalClientBootstrap.Config(clientParameter).
                Strategy<ClientStrategy>();
            await generalClientBootstrap.LaunchTaskAsync();

（3） Example GeneralUpdate.AspNetCore

    Startup.cs
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddSingleton<IUpdateService, GeneralUpdateService>();
    }

    UpdateController.cs

    private readonly ILogger<UpdateController> _logger;
    private readonly IUpdateService _updateService;

    public UpdateController(ILogger<UpdateController> logger, IUpdateService updateService)
    {
        _logger = logger;
        _updateService = updateService;
    }

    /// <summary>
    /// https://localhost:5001/api/update/getUpdateVersions/1/1.1.1
    /// </summary>
    /// <param name="clientType"> 1:ClientApp 2:UpdateApp</param>
    /// <param name="clientVersion"></param>
    /// <returns></returns>
    [HttpGet("getUpdateVersions/{clientType}/{clientVersion}")]
    public async Task<IActionResult> GetUpdateVersions(int clientType, string clientVersion)
    {
        _logger.LogInformation("Client request 'GetUpdateVersions'.");
        var resultJson = await _updateService.UpdateVersionsTaskAsync(clientType, clientVersion, UpdateVersions);
        return Ok(resultJson);
    }

    /// <summary>
    /// https://localhost:5001/api/update/getUpdateValidate/1/1.1.1
    /// </summary>
    /// <param name="clientType"> 1:ClientApp 2:UpdateApp</param>
    /// <param name="clientVersion"></param>
    /// <returns></returns>
    [HttpGet("getUpdateValidate/{clientType}/{clientVersion}")]
    public async Task<IActionResult> GetUpdateValidate(int clientType, string clientVersion)
    {
        _logger.LogInformation("Client request 'GetUpdateValidate'.");
        var lastVersion = GetLastVersion();
        var resultJson = await _updateService.UpdateValidateTaskAsync(clientType, clientVersion, lastVersion, true, GetValidateInfos);
        return Ok(resultJson);
    }

## 支持作者 ###
![](imgs/vx2.jpg)
![](imgs/alipay2.jpg)