# GeneralUpdate #
![](https://img.shields.io/badge/GeneralUpdate.Core-3.6.10-green)
![](https://img.shields.io/badge/GeneralUpdate.Common-1.0.0-green)
![](https://img.shields.io/badge/GeneralUpdate.ClientCore-1.1.2-green)
![](https://img.shields.io/badge/GeneralUpdate.AspNetCore-1.0.0-green)
![](https://img.shields.io/badge/GeneralUpdate.Zip-1.0.0-green)
![](https://img.shields.io/badge/GeneralUpdate.Single-1.0.0-green)
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

### Nuget address ###

- https://www.nuget.org/packages/GeneralUpdate.Common/
- https://www.nuget.org/packages/GeneralUpdate.ClientCore/
- https://www.nuget.org/packages/GeneralUpdate.AspNetCore/
- https://www.nuget.org/packages/GeneralUpdate.Zip/
- https://www.nuget.org/packages/GeneralUpdate.Single/
- https://www.nuget.org/packages/GeneralUpdate.Core/

### Update flow chart ###
![](imgs/flow2.png)

### Operation effect and update process display ###
![](imgs/run.jpg)

### Quick start ###

（1） Example GeneralUpdate.ClientCore

```c#
    private ClientParameter clientParameter;
    private GeneralClientBootstrap generalClientBootstrap;

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        Task.Run(async()=> 
        {
            //Main program information
            var mainVersion = "1.1.1";
            var mianType = 1;

            //This object is used for interaction between the main program client and the update component process
            clientParameter = new ClientParameter();
            //Update the component version number
            clientParameter.ClientVersion = "1.1.1";
            //Client type: 1. Main program client 2. Update components
            clientParameter.ClientType = 2;
            //Update program EXE name
            clientParameter.AppName = "AutoUpdate.ConsoleApp";
            //Main program client exe name
            clientParameter.MainAppName = "AutoUpdate.Test";
            //The application address of the local client program
            clientParameter.InstallPath = @"D:\update_test";
            //Update announcement webpage
            clientParameter.UpdateLogUrl = "https://www.baidu.com/";
            //The update component requests validation of the updated server address
            clientParameter.ValidateUrl = $"https://127.0.0.1:5001/api/update/getUpdateValidate/{ clientParameter.ClientType }/{ clientParameter.ClientVersion }";
            //Update component Update package download address
            clientParameter.UpdateUrl = $"https://127.0.0.1:5001/api/update/getUpdateVersions/{ clientParameter.ClientType }/{ clientParameter.ClientVersion }";
            //The main program client requests to validate the updated server address
            clientParameter.MainValidateUrl = $"https://127.0.0.1:5001/api/update/getUpdateValidate/{ mianType }/{ mainVersion }";
            //Main program client update package download address
            clientParameter.MainUpdateUrl = $"https://127.0.0.1:5001/api/update/getUpdateVersions/{ mianType }/{ mainVersion }";

            generalClientBootstrap = new GeneralClientBootstrap();
            //Single or multiple update pack download notification events
            generalClientBootstrap.MutiDownloadProgressChanged += OnMutiDownloadProgressChanged;
            //One or more update package download speed, remaining download events, and current download version information notification events
            generalClientBootstrap.MutiDownloadStatistics += OnMutiDownloadStatistics;
            //One or more update packages have been downloaded
            generalClientBootstrap.MutiDownloadCompleted += OnMutiDownloadCompleted;
            //Complete all download task notifications
            generalClientBootstrap.MutiAllDownloadCompleted += OnMutiAllDownloadCompleted;
            //An exception notification occurred during the download process
            generalClientBootstrap.MutiDownloadError += OnMutiDownloadError;
            //Any problems that occur throughout the update process will be notified through this event
            generalClientBootstrap.Exception += OnException;
            //ClientStrategy This update strategy performs 1. Automatic component update 2. 3. Configure ClientParameter and no longer need to write args array process to communicate as in previous version.
            generalClientBootstrap.Config(clientParameter).
                Strategy<ClientStrategy>();
            await generalClientBootstrap.LaunchTaskAsync();
        });
    }

    private void OnMutiDownloadStatistics(object sender, MutiDownloadStatisticsEventArgs e)
    {
         //e.Remaining Remaining download time
         //e.Speed Download speed
         //e.Version Information about the downloaded version
    }

    private void OnMutiDownloadProgressChanged(object sender, MutiDownloadProgressChangedEventArgs e)
    {
        //e.TotalBytesToReceive The total size of the current update package to download
        //e.ProgressValue Current progress value
        //e.ProgressPercentage Percentage of current progress
        //e.Version Information about the downloaded version
        
        //e.Type Progresype.Check Check the version. Progresype.Donwload Downloading the current version. ProgressType.Done Update completed 5.ProgressType.Fail update failed
        
        //e.BytesReceived Downloaded size
    }
```


（2） Example GeneralUpdate.Core

```c#
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
```


（3） Example GeneralUpdate.AspNetCore

```c#
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
```

## Author ###

Juster.zhu & Charles.Yu
