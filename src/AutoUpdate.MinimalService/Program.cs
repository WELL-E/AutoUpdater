using GeneralUpdate.AspNetCore.Services;
using GeneralUpdate.Common.DTOs;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<IUpdateService, GeneralUpdateService>();
var app = builder.Build();

app.MapGet("/versions/{clientType}/{clientVersion}", async (int clientType, string clientVersion,IUpdateService updateService) =>
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
    results.Add(new UpdateVersionDTO("f698f9032c0d5401bacd3b0f53099618", 1626711760, "1.1.3",
    "http://192.168.50.225/updatepacket1.zip",
    "updatepacket1"));
    results.Add(new UpdateVersionDTO("6a1046a66cedf509bfb2a771b2a7a64e", 1626711820, "1.1.4",
    "http://192.168.50.225/updatepacket2.zip",
    "updatepacket2"));
    results.Add(new UpdateVersionDTO("7689c472ce73a4b8f1b7c791731337e1", 1626711880, "1.1.5",
    "http://192.168.50.225/updatepacket3.zip",
    "updatepacket3"));
    return await Task.FromResult(results);
}

async Task<List<UpdateVersionDTO>> GetValidateInfos(int clientType, string clientVersion)
{
    //TODO:Link database query information.Different version information can be returned according to the 'clientType' of request.
    var results = new List<UpdateVersionDTO>();
    results.Add(new UpdateVersionDTO("f698f9032c0d5401bacd3b0f53099618", 1626711760, "1.1.3", null, null));
    results.Add(new UpdateVersionDTO("6a1046a66cedf509bfb2a771b2a7a64e", 1626711820, "1.1.4", null, null));
    results.Add(new UpdateVersionDTO("7689c472ce73a4b8f1b7c791731337e1", 1626711880, "1.1.5", null, null));
    return await Task.FromResult(results);
}

string GetLastVersion()
{
    //TODO:Link database query information.
    return "1.1.5";
}