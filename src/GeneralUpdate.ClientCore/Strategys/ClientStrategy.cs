using GeneralUpdate.Common.DTOs;
using GeneralUpdate.Common.Models;
using GeneralUpdate.Common.Utils;
using GeneralUpdate.Core.Strategys;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace GeneralUpdate.ClientCore.Strategys
{
    public class ClientStrategy : DefaultStrategy
    {
        public override void Excute()
        {
            if (Packet.IsUpdate)
            {
                base.Excute();
            }
            else
            {
                StartApp(Packet.AppName);
            }
        }

        protected override bool StartApp(string appName)
        {
            try
            {
                Task.Run(async () =>
                {
                    var respDTO = await HttpUtil.GetTaskAsync<UpdateValidateRespDTO>(Packet.MainValidateUrl);
                    if (respDTO.Code == 200)
                    {
                        var body = respDTO.Body;
                        if (body.IsForcibly)
                        {
                            try
                            {
                                var clientParameter = new ClientParameter();
                                clientParameter.ClientVersion = Packet.ClientVersion;
                                clientParameter.LastVersion = Packet.LastVersion;
                                clientParameter.InstallPath = Packet.InstallPath;
                                clientParameter.UpdateLogUrl = Packet.UpdateLogUrl;
                                clientParameter.UpdateUrl = Packet.MainUpdateUrl;
                                clientParameter.AppName = Packet.MainAppName;
                                clientParameter.ClientType = 1;
                                clientParameter.UpdateVersions = ConvertUtil.ToUpdateVersions(body.UpdateVersions);
                                var clientParameterBase64 = SerializeUtil.Serialize(clientParameter);
                                if (!string.IsNullOrEmpty(Packet.UpdateLogUrl))
                                {
                                    Process.Start("explorer.exe", Packet.UpdateLogUrl);
                                }
                                Process.Start($"{Packet.InstallPath}\\{appName}.exe", clientParameterBase64);
                                Process.GetCurrentProcess().Kill();
                            }
                            catch (Exception ex)
                            {
                                if (ExceptionEventAction != null)
                                    ExceptionEventAction(this, new Core.Update.ExceptionEventArgs(ex));
                            }
                        }
                    }
                    else
                    {
                        if(ExceptionEventAction != null)
                           ExceptionEventAction(this,
                            new Core.Update.ExceptionEventArgs(new System.Exception($"{ respDTO.Code }{ respDTO.Message }")));
                    }
                });
                return true;
            }
            catch (Exception ex)
            {
                if (ExceptionEventAction != null)
                    ExceptionEventAction(this, new Core.Update.ExceptionEventArgs(ex));
                return false;
            }
        }
    }
}
