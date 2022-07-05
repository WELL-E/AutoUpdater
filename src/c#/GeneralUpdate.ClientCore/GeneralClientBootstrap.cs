using GeneralUpdate.Core.Bootstrap;
using GeneralUpdate.Core.DTOs;
using GeneralUpdate.Core.Models;
using GeneralUpdate.Core.Strategys;
using GeneralUpdate.Core.Utils;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace GeneralUpdate.ClientCore
{
    public class GeneralClientBootstrap : AbstractBootstrap<GeneralClientBootstrap, IStrategy>
    {
        #region Private Members

        private Func<bool> _customOption;

        #endregion

        #region Constructors

        public GeneralClientBootstrap() : base() { }

        #endregion

        #region Public Methods

        public override async Task<GeneralClientBootstrap> LaunchTaskAsync()
        {
            try
            {
                var respDTO = await HttpUtil.GetTaskAsync<UpdateValidateRespDTO>(Packet.ValidateUrl);
                if (respDTO == null) throw new ArgumentNullException("The verification request is abnormal, please check the network or parameter configuration!");
                if (respDTO.Code != HttpStatus.OK) throw new Exception($"Request failed , Code :{ respDTO.Code }, Message:{ respDTO.Message } !");
                if (respDTO.Code == HttpStatus.OK)
                {
                    var body = respDTO.Body;
                    if(body == null) return await Task.FromResult(this);
                    Packet.IsUpdate = body.IsForcibly;
                    //Do you need to force an update.
                    if (body.IsForcibly)
                    {
                        await base.LaunchTaskAsync();
                    }
                    else if(body.IsUpdate)//Does it need to be updated.
                    {
                        bool isSkip = false;
                        //User decides if update is required.
                        if (_customOption != null) isSkip = _customOption.Invoke();
                        if (isSkip) await base.LaunchTaskAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return await Task.FromResult(this);
        }

        /// <summary>
        /// Configure server address .
        /// </summary>
        /// <param name="url">Remote server address.</param>
        /// <param name="appName">The updater name does not need to contain an extension.</param>
        /// <returns></returns>
        /// <exception cref="Exception">Parameter initialization is abnormal.</exception>
        public GeneralClientBootstrap Config(string url,string appSecretKey, string appName = "AutoUpdate.Core")
        {
            if (string.IsNullOrEmpty(url)) throw new Exception("Url cannot be empty !");
            try
            {
                string basePath = Environment.CurrentDirectory;
                Packet.InstallPath = basePath;
                Packet.IsUpdate = true;
                Packet.AppSecretKey = appSecretKey;
                //update app.
                Packet.AppName = appName;
                string clienVersion = GetFileVersion(Path.Combine(basePath, Packet.AppName + ".exe"));
                Packet.ClientVersion = clienVersion;
                Packet.AppType = (int)AppType.UpdateApp;
                Packet.ValidateUrl = $"{url}/validate/{ Packet.AppType }/{ clienVersion }/{ Packet.AppSecretKey }";
                Packet.UpdateUrl = $"{url}/versions/{ Packet.AppType }/{ clienVersion }/{ Packet.AppSecretKey }";
                //main app.
                string mainAppName = Path.GetFileNameWithoutExtension(Process.GetCurrentProcess().MainModule.FileName);
                string mainVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
                Packet.MainValidateUrl = $"{url}/validate/{ (int)AppType.ClientApp }/{ mainVersion }/{Packet.AppSecretKey}";
                Packet.MainUpdateUrl = $"{url}/versions/{ (int)AppType.ClientApp }/{ mainVersion }/{Packet.AppSecretKey}";
                Packet.MainAppName = mainAppName;
                return this;
            }
            catch (Exception ex)
            {
                throw new Exception($"Initial configuration parameters are abnormal . {  ex.Message }", ex.InnerException);
            }
        }

        public GeneralClientBootstrap Config(ClientParameter clientParameter)
        {
            ValidateConfig(clientParameter);
            Packet.ClientVersion = clientParameter.ClientVersion;
            Packet.AppType = clientParameter.AppType;
            Packet.ValidateUrl = clientParameter.ValidateUrl;
            Packet.UpdateUrl = clientParameter.UpdateUrl;
            Packet.MainValidateUrl = clientParameter.MainValidateUrl;
            Packet.MainUpdateUrl = clientParameter.MainUpdateUrl;
            Packet.AppName = clientParameter.AppName;
            Packet.MainAppName = clientParameter.MainAppName;
            Packet.InstallPath = clientParameter.InstallPath;
            Packet.UpdateLogUrl = clientParameter.UpdateLogUrl;
            Packet.IsUpdate = clientParameter.IsUpdate;
            Packet.AppSecretKey = clientParameter.AppSecretKey;
            return this;
        }

        /// <summary>
        /// Let the user decide whether to update in the state of non-mandatory update.
        /// </summary>
        /// <param name="func">Custom funcion ,Custom actions to let users decide whether to update. true update false do not update .</param>
        /// <returns></returns>
        public GeneralClientBootstrap SetCustomOption(Func<bool> func)
        {
            _customOption = func;
            return this;
        }

        #endregion

        #region Private Methods

        private void ValidateConfig(ClientParameter clientParameter)
        {
            if (clientParameter == null) throw new NullReferenceException("Client parameter not set.");

            if (string.IsNullOrEmpty(clientParameter.ClientVersion)) throw new NullReferenceException("Client version not set.");

            if (string.IsNullOrEmpty(clientParameter.InstallPath)) throw new NullReferenceException("Install path not set.");

            if (string.IsNullOrEmpty(clientParameter.UpdateUrl))
            {
                throw new NullReferenceException("Update url not set.");
            }
            else if (!DataValidateUtil.IsURL(clientParameter.UpdateUrl))
            {
                throw new NullReferenceException("Illegal url address.");
            }

            if (string.IsNullOrEmpty(clientParameter.ValidateUrl))
            {
                throw new NullReferenceException("Update url not set.");
            }
            else if (!DataValidateUtil.IsURL(clientParameter.ValidateUrl))
            {
                throw new NullReferenceException("Illegal url address.");
            }

            if (string.IsNullOrEmpty(clientParameter.AppName)) throw new NullReferenceException("Main app name not set.");
            if (string.IsNullOrEmpty(clientParameter.AppSecretKey)) throw new NullReferenceException("You need to specify any unique string as the APP key !");
        }

        private string GetFileVersion(string filePath)
        {
            try
            {
                var fileInfo = new FileInfo(filePath);
                if (fileInfo != null && fileInfo.Exists) return FileVersionInfo.GetVersionInfo(filePath).FileVersion;
                throw new Exception($"Failed to obtain file '{ filePath }' version. Procedure.");
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to obtain file '{ filePath }' version. Procedure. Eorr message : { ex.Message } .", ex.InnerException);
            }
        }

        #endregion
    }
}