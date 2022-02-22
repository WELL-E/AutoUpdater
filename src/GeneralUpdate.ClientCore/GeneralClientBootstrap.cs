using GeneralUpdate.ClientCore.Bootstrap;
using GeneralUpdate.ClientCore.DTOs;
using GeneralUpdate.ClientCore.Models;
using GeneralUpdate.ClientCore.Utils;
using System;
using System.Threading.Tasks;
using GeneralUpdate.ClientCore.Strategies;

namespace GeneralUpdate.ClientCore
{
    public class GeneralClientBootstrap : AbstractBootstrap<GeneralClientBootstrap, IStrategy>
    {
        public GeneralClientBootstrap() : base()
        {
        }

        public override async Task<GeneralClientBootstrap> LaunchTaskAsync()
        {
            try
            {
                var respDTO = await HttpUtil.GetTaskAsync<UpdateValidateRespDTO>(Packet.ValidateUrl);
                if (respDTO.Code == 200)
                {
                    var body = respDTO.Body;
                    Packet.IsUpdate = body.IsForcibly;
                    if (body.IsForcibly)
                    {
                        await base.LaunchTaskAsync();
                    }
                    else
                    {
                        base.ExcuteStrategy();
                    }
                }
                else
                {
                    throw new Exception($"{ respDTO.Code },{ respDTO.Message }.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message,ex);
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
            string basePath =  System.Environment.CurrentDirectory;
            string mainAppName = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
            string clienVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            Packet.ClientVersion = clienVersion;
            Packet.ClientType = 1;
            Packet.ValidateUrl = url;
            Packet.UpdateUrl = url;
            Packet.MainValidateUrl = url;
            Packet.MainUpdateUrl = url;
            Packet.AppName = "GeneralUpdate";
            Packet.MainAppName = mainAppName;
            Packet.InstallPath = basePath;
            Packet.UpdateLogUrl = null;
            Packet.IsUpdate = true;
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
    }
}
