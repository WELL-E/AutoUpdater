using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GeneralUpdate.ClientCore.Hubs
{
    public class VersionHub
    {
        private HubConnection connection = null;
        private const string ClientName = "GeneralUpdate.Client";

        public VersionHub() 
        {
            //初始化SignalR的hub，然后指定服务器地址
            connection = new HubConnectionBuilder()
               .WithUrl("https://localhost:44394/chathub")
               //重连机制
               .WithAutomaticReconnect(new RandomRetryPolicy())
               .Build();
            //关闭连接
            connection.Closed += async (error) =>
            {
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await connection.StartAsync();
            };

            //重连
            connection.Reconnecting += error =>
            {
                ////TODO:重连处理
                // Notify users the connection was lost and the client is reconnecting.
                // Start queuing or dropping messages.
                return Task.CompletedTask;
            };

            //接收消息
            connection.On<string, string>("ReceiveMessage", (user, message) =>
            {
                //TODO:接收到新版本推送处理
            });

            //离线、上线通知
            connection.On<string>("online", (message) =>
            {
                //TODO:离线、上线通知处理
            });

            connection.StartAsync();
        }

        /// <summary>
        /// 发送消息给服务器
        /// </summary>
        /// <param name="user">用户名</param>
        /// <param name="msg">消息内容</param>
        /// <returns></returns>
        public async Task Send(string user, string msg)
        {
            try
            {
                await connection.InvokeAsync("SendMessage",
                    user, msg);
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// 上线
        /// </summary>
        /// <returns></returns>
        public async Task Login()
        {
            await connection.InvokeAsync("Login", ClientName);
        }

        /// <summary>
        /// 离线
        /// </summary>
        /// <returns></returns>
        public async Task SignOut()
        {
            await connection.InvokeAsync("SignOut", ClientName);
            await connection.StopAsync();
        }
    }
}