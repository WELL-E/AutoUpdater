using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace GeneralUpdate.AspNetCore.Hubs
{
    public class VersionHub : Hub
    {
        //TODO:需继续完善
        private const string ReceiveMessageflag = "ReceiveMessage";
        private const string ClientNameflag = "GeneralUpdate.Client";
        private const string Groupflag = "Groupflag";

        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(string user, string message)
        {
            if(string.IsNullOrWhiteSpace(user) || string.IsNullOrWhiteSpace(message)) 
                throw new ArgumentNullException($"'VersionHub' The required parameter send message cannot be null !");

            try
            {
                await Clients.All.SendAsync(ReceiveMessageflag, user, message);
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
            if(string.IsNullOrWhiteSpace(connectionId)) 
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

        public async Task Login(string name)
        {
            try
            {
                await Clients.AllExcept(Context.ConnectionId).SendAsync("online", $"{ name }in the group.");
                await Clients.Group(Groupflag).SendAsync("online", $"{ name }in the group.");
            }
            catch (Exception ex)
            {
                throw new Exception($"'VersionHub' Login error :  { ex.Message } .", ex.InnerException);
            }
        }

        public async Task SignOut(string name)
        {
            try
            {
                await Clients.AllExcept(Context.ConnectionId).SendAsync("online", $"{ name }leave the group .");
            }
            catch (Exception ex)
            {
                throw new Exception($"'VersionHub' Sign out error :  { ex.Message }", ex.InnerException);
            }
        }
    }
}