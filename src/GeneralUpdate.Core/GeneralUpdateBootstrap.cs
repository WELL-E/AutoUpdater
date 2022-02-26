using GeneralUpdate.Common.Models;
using GeneralUpdate.Common.Utils;
using GeneralUpdate.Core.Bootstrap;
using GeneralUpdate.Core.Strategys;
using GeneralUpdate.Core.Utils;
using System;

namespace GeneralUpdate.Core
{
    public class GeneralUpdateBootstrap : AbstractBootstrap<GeneralUpdateBootstrap, IStrategy>
    {
        public ClientParameter ClientParameter { get; set; }

        public GeneralUpdateBootstrap() : base()
        {
        }

        /// <summary>
        /// Set parameter.
        /// </summary>
        /// <param name="clientParameter">ClientParameter object to base64 string.</param>
        /// <returns></returns>
        public GeneralUpdateBootstrap RemoteAddressBase64(string clientParameter)
        {
            try
            {
                ClientParameter = SerializeUtil.Deserialize<ClientParameter>(clientParameter);
                ValidateRemoteAddress(ClientParameter);
                InitPacket();
            }
            catch (Exception ex)
            {
                throw new Exception($"Client parameter json conversion failed, please check whether the parameter content is legal : { ex.Message },{ ex.StackTrace }.");
            }
            return this;
        }

        private void InitPacket()
        {
            Packet.ClientVersion = ClientParameter.ClientVersion;
            Packet.LastVersion = ClientParameter.LastVersion;
            Packet.AppName = ClientParameter.AppName;
            Packet.InstallPath = ClientParameter.InstallPath;
            Packet.TempPath = $"{FileUtil.GetTempDirectory(Packet.LastVersion)}\\";
            Packet.IsUpdate = ClientParameter.IsUpdate;
            Packet.UpdateUrl = ClientParameter.UpdateUrl;
            Packet.UpdateLogUrl = ClientParameter.UpdateLogUrl;
            Packet.UpdateVersions = ClientParameter.UpdateVersions;
            Packet.CompressEncoding = ConvertUtil.ToEncoding(ClientParameter.CompressEncoding);
            Packet.CompressFormat = ClientParameter.CompressFormat;
            Packet.DownloadTimeOut = ClientParameter.DownloadTimeOut;
            Packet.MainUpdateUrl= ClientParameter.MainUpdateUrl;
            Packet.MainValidateUrl= ClientParameter.MainValidateUrl;
            Packet.AppType= ClientParameter.AppType;
        }

        private void ValidateRemoteAddress(ClientParameter clientParameter)
        {
            if (clientParameter == null)
            {
                throw new NullReferenceException("Client parameter not set.");
            }

            if (string.IsNullOrEmpty(clientParameter.ClientVersion))
            {
                throw new NullReferenceException("Client version not set.");
            }

            if (string.IsNullOrEmpty(clientParameter.LastVersion))
            {
                throw new NullReferenceException("Last version not set.");
            }

            if (string.IsNullOrEmpty(clientParameter.InstallPath))
            {
                throw new NullReferenceException("Install path not set.");
            }

            if (string.IsNullOrEmpty(clientParameter.AppName))
            {
                throw new NullReferenceException("Main app name not set.");
            }

            if (clientParameter.UpdateVersions == null || clientParameter.UpdateVersions.Count == 0)
            {
                throw new NullReferenceException("Update versions not set.");
            }
        }
    }
}