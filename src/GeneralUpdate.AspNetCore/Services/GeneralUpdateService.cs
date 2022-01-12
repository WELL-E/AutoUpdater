using GeneralUpdate.AspNetCore.Models;
using GeneralUpdate.Common.DTOs;
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
        public async Task<string> UpdateValidateTaskAsync(int clientType,string clientVersion, string serverLastVersion, 
            bool isForce, Func<int,string, Task<List<UpdateVersionDTO>>> getUrlsAction)
        {
            if (getUrlsAction == null)
            {
                throw new ArgumentNullException(@"'getUrlsAction' cannot be null!");
            }
            ParameterVerification(clientType, clientVersion, serverLastVersion);
            Version clientLastVersion;
            var respDTO = new UpdateValidateRespDTO();
            try
            {
                if (!Version.TryParse(clientVersion, out clientLastVersion))
                {
                    respDTO.Message = $"{ RespMessage.RequestFailed } Wrong version number.";
                    respDTO.Code = 400;
                    return null;
                }
                var lastVersion = new Version(serverLastVersion);
                if (clientLastVersion < lastVersion)
                {
                    respDTO.Body = new UpdateValidateDTO();
                    var body = respDTO.Body;
                    body.ClientType = clientType;
                    body.UpdateVersions = await getUrlsAction(clientType,clientVersion);
                    body.IsForcibly = isForce;
                    respDTO.Code = 200;
                    respDTO.Message = RespMessage.RequestSucceeded;
                }
                else
                {
                    respDTO.Code = 200;
                    respDTO.Message = RespMessage.RequestNone;
                }
            }
            catch
            {
                respDTO.Message = RespMessage.ServerException;
                respDTO.Code = 500;
            }
            return JsonConvert.SerializeObject(respDTO);
        }

        /// <summary>
        /// Return all updated version information.
        /// </summary>
        /// <param name="getUrlsAction">Each version update (Query the latest version information in the database according to the client version number).</param>
        /// <returns></returns>
        public async Task<string> UpdateVersionsTaskAsync(int clientType, string clientVersion,Func<int,string,Task<List<UpdateVersionDTO>>> getUrlsAction) 
        {
            if (getUrlsAction == null) throw new ArgumentNullException(@"'getUrlsAction' cannot be null!");

            var respDTO = new UpdateVersionsRespDTO();
            try
            {
                respDTO.Code = 200;
                respDTO.Message = RespMessage.RequestSucceeded;
                respDTO.Body = new UpdateVersionsDTO { UpdateVersions = await getUrlsAction(clientType, clientVersion) };
            }
            catch (Exception)
            {
                respDTO.Code = 500;
                respDTO.Message = RespMessage.ServerException;
            }
            return JsonConvert.SerializeObject(respDTO);
        }

        private void ParameterVerification(int clientType,string clientVersion, string serverLastVersion)
        {
            if (clientType == 0)
            {
                throw new Exception(@"'clientType' cannot both be 0!");
            }
            else if (string.IsNullOrWhiteSpace(clientVersion))
            {
                throw new ArgumentNullException(@"'clientVersion' cannot be null!");
            }
            else if (string.IsNullOrWhiteSpace(serverLastVersion))
            {
                throw new ArgumentNullException(@"'serverLastVersion' cannot be null!");
            }
        }

        /// <summary>
        /// Production differential package.
        /// </summary>
        /// <param name="oldPath">The folder path of the previous version.</param>
        /// <param name="newPath">The folder path of the latest version.</param>
        /// <param name="targetPath">Production differential package the path.</param>
        /// <returns></returns>
        public Task DifferentialPackage(string oldPath,string newPath,string targetPath) 
        {
            /*
             * TODO: 
             *    1.初始化好生成的路径
             *    2.通过树形结构管理新旧版本
             *    3.遍历树形结构根据文件的MD5码和具体的文件版本信息进行比对
             *    4.遍历筛选完成后按照原来的文件树结构生成好差分包
             *    5.向数据库插入差分包的版本信息
             *    6.生成好更新日志便于版本管理
             */
            return Task.CompletedTask;
        }

        /// <summary>
        /// Push the latest version of the content to the client through Signalr.
        /// </summary>
        /// <param name="cmd">Push the instructions that need to be executed(JSON).</param>
        /// <returns></returns>
        public Task PushLastVersion(Func<IEnumerable<UpdateVersionDTO>> getUpdateVersion) 
        {
            /*
             * TODO：
             *    1.从数据库中读取需要推送的版本信息
             */
            return Task.CompletedTask;
        }

        /// <summary>
        /// Push the command.
        /// </summary>
        /// <param name="cmd">JSON</param>·
        /// <returns></returns>
        public Task PushCommand(string cmd)
        {
            /*
             * TODO：
             *    1.从数据库中读取需要推送的版本信息
             *    2.并制定更新的指令，通过‘cmd’来表示
             *    3.开放推送指令，用于推送临时补救意外情况。例如：更新包压缩了错误文件导致失败
             */
            return Task.CompletedTask;
        }
    }
}
