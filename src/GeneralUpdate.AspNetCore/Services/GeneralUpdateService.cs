using GeneralUpdate.AspNetCore.Models;
using GeneralUpdate.Core.DTOs;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
                    body.UpdateVersions = await getUrlsAction(clientType, clientVersion);
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
        public async Task<string> UpdateVersionsTaskAsync(int clientType, string clientVersion, Func<int, string, Task<List<UpdateVersionDTO>>> getUrlsAction)
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

        /// <summary>
        /// TODO: To be perfected, not available.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="insertLastVserion"></param>
        /// <returns></returns>
        public async Task UploadPatchPacket(HttpContext context, Func<bool> insertLastVserion)
        {
            //file root dir path 文件保存目录路径
            var savePath = "/upload/";
            //定义允许上传的文件扩展名
            var extTable = new Hashtable();
            extTable.Add("zip", "zip,7z");
            //最大文件大小
            var maxSize = 1000000;
            var imgFile = context.Request.Form.Files["zipfile"];
            if (imgFile?.FileName == null)
            {
                //await ShowError("请选择文件。");
                return;
            }

            var dirName = context.Request.Query["dir"][0];
            if (string.IsNullOrEmpty(dirName))
            {
                dirName = "image";
            }
            if (!extTable.ContainsKey(dirName))
            {
                //await ShowError("目录名不正确。");
                return;
            }
            var fileExt = Path.GetExtension(imgFile.FileName).ToLower();
            if (imgFile.Length > maxSize)
            {
                //await ShowError("上传文件大小超过限制。");
                return;
            }
            if (string.IsNullOrEmpty(fileExt) ||
                Array.IndexOf(((String)extTable[dirName]).Split(','), fileExt.Substring(1).ToLower()) == -1)
            {
                //await ShowError($"上传文件扩展名是不允许的扩展名。\n只允许{extTable[dirName]}格式。");
            }
            savePath += dirName + "/";
            var ymd = DateTime.UtcNow.ToString("yyyyMM");
            savePath += ymd + "/";

            var newFileName = DateTime.UtcNow.ToString("yyyyMMddHHmmss") + fileExt;
            var filePath = savePath + newFileName;
            //save file
            using (var stream = new MemoryStream())
            {
                await imgFile.CopyToAsync(stream);

                //var fileUrl = await _storageProvider.SaveBytes(stream.ToArray(), filePath);
                var fileUrl = "";
                if (!string.IsNullOrEmpty(fileUrl))
                {
                    //await context.Response.Body.WriteAsync(new { error = 0, url = fileUrl }.ToJson().GetBytes());
                }
                else
                {
                    //await ShowError("上传图片失败");
                }
            }
        }

        private void ParameterVerification(int clientType, string clientVersion, string serverLastVersion)
        {
            if (clientType <= 0) throw new Exception(@"'clientType' cannot be less than or equal to 0 !");
            if (string.IsNullOrWhiteSpace(clientVersion)) throw new ArgumentNullException(@"'clientVersion' cannot be null !");
            if (string.IsNullOrWhiteSpace(serverLastVersion)) throw new ArgumentNullException(@"'serverLastVersion' cannot be null !");
        }
    }
}