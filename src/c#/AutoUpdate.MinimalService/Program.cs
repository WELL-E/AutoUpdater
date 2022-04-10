using GeneralUpdate.AspNetCore.Hubs;
using GeneralUpdate.AspNetCore.Services;
using GeneralUpdate.Core.DTOs;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<IUpdateService, GeneralUpdateService>();
builder.Services.AddSignalR();
var app = builder.Build();

app.MapHub<VersionHub>("/versionhub");

//app.Use(async (context, next) =>
//{
//    var hubContext = context.RequestServices.GetRequiredService<IHubContext<VersionHub>>();
//    await CommonHubContextMethod((IHubContext)hubContext);
//    if (next != null)
//    {
//        await next.Invoke();
//    }
//});

//async Task CommonHubContextMethod(IHubContext context)
//{
//    await context.Clients.All.SendAsync("clientMethod","");
//}

app.MapGet("/versions/{clientType}/{clientVersion}", async (int clientType, string clientVersion, IUpdateService updateService) =>
{
    return await updateService.UpdateVersionsTaskAsync(clientType, clientVersion, UpdateVersions);
});

app.MapGet("/validate/{clientType}/{clientVersion}", async (int clientType, string clientVersion, IUpdateService updateService) =>
{
    return await updateService.UpdateValidateTaskAsync(clientType, clientVersion, GetLastVersion(), true, GetValidateInfos);
});
app.Run();

async Task<List<UpdateVersionDTO>> UpdateVersions(int clientType, string clientVersion)
{
    //TODO:Link database query information.Different version information can be returned according to the 'clientType' of request.
    var results = new List<UpdateVersionDTO>();
    results.Add(new UpdateVersionDTO("5001fd3732b91dfe46196ceb0a5bc4b2", 1626711760, "9.1.3.0",
    "http://192.168.50.170/patchs.zip",
    "updatepacket1"));
    //results.Add(new UpdateVersionDTO("d9a3785f08ed3dd92872bd807ebfb917", 1626711820, "9.1.4.0",
    //"http://192.168.50.170/Update2.zip",
    //"updatepacket2"));
    //results.Add(new UpdateVersionDTO("224da586553d60315c55e689a789b7bd", 1626711880, "9.1.5.0",
    //"http://192.168.50.170/Update3.zip",
    //"updatepacket3"));
    return await Task.FromResult(results);
}

async Task<List<UpdateVersionDTO>> GetValidateInfos(int clientType, string clientVersion)
{
    //TODO:Link database query information.Different version information can be returned according to the 'clientType' of request.
    var results = new List<UpdateVersionDTO>();
    results.Add(new UpdateVersionDTO("5001fd3732b91dfe46196ceb0a5bc4b2", 1626711760, "9.1.3.0", null, null));
    //results.Add(new UpdateVersionDTO("d9a3785f08ed3dd92872bd807ebfb917", 1626711820, "9.1.4.0", null, null));
    //results.Add(new UpdateVersionDTO("224da586553d60315c55e689a789b7bd", 1626711880, "9.1.5.0", null, null));
    return await Task.FromResult(results);
}

string GetLastVersion()
{
    //TODO:Link database query information.
    return "1.1.5";
}