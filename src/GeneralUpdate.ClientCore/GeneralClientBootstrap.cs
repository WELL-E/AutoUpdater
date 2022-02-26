using GeneralUpdate.Common.DTOs;
using GeneralUpdate.Common.Models;
using GeneralUpdate.Common.Utils;
using GeneralUpdate.Core.Bootstrap;
using GeneralUpdate.Core.Strategys;
using System;
using System.IO;
using System.Threading.Tasks;

namespace GeneralUpdate.ClientCore
{
    public class GeneralClientBootstrap : AbstractBootstrap<GeneralClientBootstrap, IStrategy>
    {
        private const int ClientType = 1, MianType = 2;

        public GeneralClientBootstrap() : base()
        {
        }

        public override async Task<GeneralClientBootstrap> LaunchAsync()
        {
            try
            {
                var respDTO = await HttpUtil.GetTaskAsync<UpdateValidateRespDTO>(Packet.ValidateUrl);
                if(respDTO == null || respDTO.Code != 200) throw new Exception($"{ respDTO.Code },{ respDTO.Message }.");
                if (respDTO.Code == 200)
                {
                    var body = respDTO.Body;
                    Packet.IsUpdate = body.IsForcibly;
                    if (body.IsForcibly)
                    {
                        await base.LaunchAsync();
                    }
                    else
                    {
                        base.ExcuteStrategy();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            return await Task.FromResult(this);
        }

        /// <summary>
        /// Configure server address .
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public GeneralClientBootstrap Config(string url)
        {
            string basePath = System.Environment.CurrentDirectory;
            Packet.InstallPath = basePath;
            Packet.IsUpdate = true;
            
            //update app.
            Packet.AppName = "AutoUpdate.Core";
            string clienVersion = GetFileVersion(Path.Combine(basePath, Packet.AppName));
            Packet.ClientVersion = clienVersion;
            Packet.ClientType = ClientType;
            Packet.ValidateUrl = $"{url}/validate/{ Packet.ClientType }/{ clienVersion }";
            Packet.UpdateUrl = $"{url}/versions/{ Packet.ClientType }/{ clienVersion }";
            
            //main app.
            string mainAppName = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
            string mainVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            Packet.MainValidateUrl = $"{url}/validate/{ MianType }/{ mainVersion }";
            Packet.MainUpdateUrl = $"{url}/versions/{ MianType }/{ mainVersion }";
            Packet.MainAppName = mainAppName;
            return this;
        }

        public GeneralClientBootstrap Config(ClientParameter clientParameter)
        {
            ValidateConfig(clientParameter);
            Packet.ClientVersion = clientParameter.ClientVersion;
            Packet.ClientType = clientParameter.ClientType;
            Packet.ValidateUrl = clientParameter.ValidateUrl;
            Packet.UpdateUrl = clientParameter.UpdateUrl;
            Packet.MainValidateUrl = clientParameter.MainValidateUrl;
            Packet.MainUpdateUrl = clientParameter.MainUpdateUrl;
            Packet.AppName = clientParameter.AppName;
            Packet.MainAppName = clientParameter.MainAppName;
            Packet.InstallPath = clientParameter.InstallPath;
            Packet.UpdateLogUrl = clientParameter.UpdateLogUrl;
            Packet.IsUpdate = clientParameter.IsUpdate;
            return this;
        }

        private void ValidateConfig(ClientParameter clientParameter)
        {
            if (clientParameter == null)
            {
                throw new NullReferenceException("Client parameter not set.");
            }

            if (string.IsNullOrEmpty(clientParameter.ClientVersion))
            {
                throw new NullReferenceException("Client version not set.");
            }

            if (string.IsNullOrEmpty(clientParameter.InstallPath))
            {
                throw new NullReferenceException("Install path not set.");
            }

            if (string.IsNullOrEmpty(clientParameter.UpdateUrl))
            {
                throw new NullReferenceException("Update url not set.");
            }
            else if (!DataValidateUtil.IsURL(clientParameter.UpdateUrl))
            {
                throw new Exception("Illegal url address.");
            }

            if (string.IsNullOrEmpty(clientParameter.ValidateUrl))
            {
                throw new NullReferenceException("Update url not set.");
            }
            else if (!DataValidateUtil.IsURL(clientParameter.ValidateUrl))
            {
                throw new Exception("Illegal url address.");
            }

            if (string.IsNullOrEmpty(clientParameter.AppName))
            {
                throw new NullReferenceException("Main app name not set.");
            }
        }

        private string GetFileVersion(string filePath) 
        {
            try
            {
                var fileInfo = new FileInfo(filePath);
                if (fileInfo != null && fileInfo.Exists)
                {
                    //"文件版本=" + info.FileVersion
                    //"产品版本=" + info.ProductVersion
                    //通常版本号显示为「主版本号.次版本号.生成号.专用部件号」 "系统显示文件版本：" + info.ProductMajorPart + '.' + info.ProductMinorPart + '.' + info.ProductBuildPart + '.' + info.ProductPrivatePart
                    var info = System.Diagnostics.FileVersionInfo.GetVersionInfo(filePath);
                    return info.FileVersion;
                }
                throw new Exception($"Failed to obtain file '{ filePath }' version. Procedure.");
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to obtain file '{ filePath }' version. Procedure. Eorr message : { ex.Message } .", ex.InnerException);
            }
        }
    }
}