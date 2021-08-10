using GeneralUpdate.AspNetCore.Services;
using GeneralUpdate.Common.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutoUpdate.Service.Controllers
{
    [ApiController]
    [Route("api/update")]
    public class UpdateController : Controller
    {
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

        private async Task<List<UpdateVersionDTO>> UpdateVersions(int clientType,string clientVersion)
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

        private async Task<List<UpdateVersionDTO>> GetValidateInfos(int clientType,string clientVersion)
        {
            //TODO:Link database query information.Different version information can be returned according to the 'clientType' of request.
            var results = new List<UpdateVersionDTO>();
            results.Add(new UpdateVersionDTO("f698f9032c0d5401bacd3b0f53099618", 1626711760, "1.1.3", null, null));
            results.Add(new UpdateVersionDTO("6a1046a66cedf509bfb2a771b2a7a64e", 1626711820, "1.1.4", null, null));
            results.Add(new UpdateVersionDTO("7689c472ce73a4b8f1b7c791731337e1", 1626711880, "1.1.5", null, null));
            return await Task.FromResult(results);
        }

        private string GetLastVersion()
        {
            //TODO:Link database query information.
            return "1.1.5";
        }
    }
}
