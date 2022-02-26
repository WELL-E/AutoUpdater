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
        private const string ClientNameflag = "GeneralUpdate.Client";
        private const string Groupflag = "Groupflag";

        public delegate void ConnectionStatus(HubStatus hubStatus, string message);
        public event ConnectionStatus OnConnectionStatus;

        #endregion

        #region Public Properties

        public VersionHub() { }

        #endregion

        #region Public Methods

        public override Task OnConnectedAsync()
        {
            OnConnectionStatus(HubStatus.Connected, "The Version hub is connected .");
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            OnConnectionStatus(HubStatus.Disconnected,"The Version hub is disconnected !");
            return base.OnDisconnectedAsync(exception);
        }

        /// <summary>
        /// Push the latest version information.
        /// </summary>
        /// <typeparam name="TParameter">E.g : Client parameter.</typeparam>
        /// <param name="user"></param>
        /// <param name="message">version information.</param>
        /// <returns> Empty task.</returns>
        /// <exception cref="ArgumentNullException">Null parameter anomaly.</exception>
        /// <exception cref="Exception">SignalR component related exception.</exception>
        public async Task SendMessage<TParameter>(string user, string message) where TParameter : class
        {
            if (string.IsNullOrWhiteSpace(user) || string.IsNullOrWhiteSpace(message))
                throw new ArgumentNullException($"'VersionHub' The required parameter send message cannot be null !");

            try
            {
                var clientParameter = SerializeUtil.Serialize(message);
                await Clients.All.SendAsync(ReceiveMessageflag, user, clientParameter);
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
                await Groups.RemoveFromGroupAsync(connectionId, Groupflag);
            }
            catch (Exception ex)
            {
                throw new Exception($"'VersionHub' Remove error :  { ex.Message } .", ex.InnerException);
            }
        }

        #endregion
    }
}