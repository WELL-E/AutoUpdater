using GeneralUpdate.AspNetCore.Models;
using GeneralUpdate.Core.DTOs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GeneralUpdate.AspNetCore.Services
{
    public class GeneralUpdateService : IUpdateService
    {
        /// <summary>
        /// Update validate.
        /// </summary>
        /// <param name="clientType">1:ClientApp 2:UpdateApp</param>
        /// <param name="clientVersion">The current version number of the client.</param>
        /// <param name="serverLastVersion">The latest version number of the server.</param>
        /// <param name="isForce">Do you need to force an update.</param>
        /// <param name="getUrlsAction">Each version update (Query the latest version information in the database according to the client version number).</param>
        /// <returns></returns>
        public async Task<string> UpdateValidateTaskAsync(int clientType, string clientVersion, string serverLastVersion,
            bool isForce, Func<int, string, Task<List<UpdateVersionDTO>>> getUrlsAction)
        {
            if (getUrlsAction == null) throw new ArgumentNullException(@"'getUrlsAction' cannot be null!");
            ParameterVerification(clientType, clientVersion, serverLastVersion);
            Version clientLastVersion;
            var respDTO = new UpdateValidateRespDTO();
            try
            {
                if (!Version.TryParse(clientVersion, out clientLastVersion))
                {
                    respDTO.Message = $"{ RespMessage.RequestFailed } Wrong version number.";
                    respDTO.Code = ResponseStatus.Bad;
                    return null;
                }
                var lastVersion = new Version(serverLastVersion);
                if (clientLastVersion < lastVersion)
                {
                    respDTO.Body = new UpdateValidateDTO();
                    var body = respDTO.Body;
                    body.ClientType = clientType;
                    body.UpdateVersions = await getUrlsAction(clientType, clientVersion);
                    body.IsForcibly = isForce;
                    respDTO.Code = ResponseStatus.Success;
                    respDTO.Message = RespMessage.RequestSucceeded;
                }
                else
                {
                    respDTO.Code = ResponseStatus.Success;
                    respDTO.Message = RespMessage.RequestNone;
                }
            }
            catch
            {
                respDTO.Message = RespMessage.ServerException;
                respDTO.Code = ResponseStatus.Internal;
            }
            return JsonConvert.SerializeObject(respDTO);
        }

        /// <summary>
        /// Return all updated version information.
        /// </summary>
        /// <param name="getUrlsAction">Each version update (Query the latest version information in the database according to the client version number).</param>
        /// <returns></returns>
        public async Task<string> UpdateVersionsTaskAsync(int clientType, string clientVersion, Func<int, string, Task<List<UpdateVersionDTO>>> getUrlsAction)
        {
            if (getUrlsAction == null) throw new ArgumentNullException(@"'getUrlsAction' cannot be null!");

            var respDTO = new UpdateVersionsRespDTO();
            try
            {
                respDTO.Code = ResponseStatus.Success;
                respDTO.Message = RespMessage.RequestSucceeded;
                respDTO.Body = new UpdateVersionsDTO { UpdateVersions = await getUrlsAction(clientType, clientVersion) };
            }
            catch (Exception)
            {
                respDTO.Code = ResponseStatus.Internal;
                respDTO.Message = RespMessage.ServerException;
            }
            return JsonConvert.SerializeObject(respDTO);
        }

        private void ParameterVerification(int clientType, string clientVersion, string serverLastVersion)
        {
            if (clientType <= 0) throw new Exception(@"'clientType' cannot be less than or equal to 0 !");
            if (string.IsNullOrWhiteSpace(clientVersion)) throw new ArgumentNullException(@"'clientVersion' cannot be null !");
            if (string.IsNullOrWhiteSpace(serverLastVersion)) throw new ArgumentNullException(@"'serverLastVersion' cannot be null !");
        }
    }
}