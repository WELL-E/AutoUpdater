using GeneralUpdate.Common.Utils;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace GeneralUpdate.AspNetCore.Hubs
{
    public enum HubStatus 
    {
        Connected = 1,
        Disconnected = 2
    }

    public class VersionHub : Hub
    {
        #region Private Members

        private const string ReceiveMessageflag = "ReceiveMessage";
        private const string SendMessageflag = "SendMessage";
        private const string Onlineflag = "Online";
        private const string GroupName = "VersionGroup";

        public delegate void ConnectionStatus(HubStatus hubStatus, string message);
        public event ConnectionStatus OnConnectionStatus;

        #endregion

        #region Public Properties

        public VersionHub() 
        {
        }

        #endregion

        #region Public Methods

        public async override Task OnConnectedAsync()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, GroupName);
            await base.OnConnectedAsync();
            await SendMessage("TESTNAME","zhuzhen");
            if (OnConnectionStatus != null)   OnConnectionStatus(HubStatus.Connected, "The Version hub is connected .");
        }

        public async override Task OnDisconnectedAsync(Exception exception)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, GroupName);
            await base.OnDisconnectedAsync(exception);
            if (OnConnectionStatus != null) OnConnectionStatus(HubStatus.Disconnected, "The Version hub is disconnected !");
        }

        public async Task SendMessage(string user, string message)
        {
            if (string.IsNullOrWhiteSpace(user) || string.IsNullOrWhiteSpace(message))
                throw new ArgumentNullException($"'VersionHub' The required parameter send message cannot be null !");

            try
            {
                var clientParameter = SerializeUtil.Serialize(message);
                await Clients.Groups(GroupName).SendAsync(ReceiveMessageflag, user, clientParameter);
            }
            catch (Exception ex)
            {
                throw new Exception($"'VersionHub' Send message error :  { ex.Message } .", ex.InnerException);
            }
        }

        /// <summary>
        /// Kick client connection from group.
        /// </summary>
        /// <param name="connectionId">client connectionId.</param>
        /// <returns></returns>
        public async Task Remove(string connectionId)
        {
            if (string.IsNullOrWhiteSpace(connectionId))
                throw new ArgumentNullException($"'VersionHub' The required parameter remove cannot be null !");

            try
            {
                await Groups.RemoveFromGroupAsync(connectionId, GroupName);
            }
            catch (Exception ex)
            {
                throw new Exception($"'VersionHub' Remove error :  { ex.Message } .", ex.InnerException);
            }
        }

        public Task ThrowException()
        {
            throw new HubException("This error will be sent to the client!");
        }

        #endregion
    }
}