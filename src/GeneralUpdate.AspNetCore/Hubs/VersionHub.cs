using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace GeneralUpdate.AspNetCore.Hubs
{
    internal class VersionHub : Hub
    {
        //TODO: SignalR

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task Login(string name)
        {
            await Clients.AllExcept(Context.ConnectionId).SendAsync("online", $"{ name }in the group.");
            await Clients.Group("JusterGroup").SendAsync("online", $"{ name }in the group.");
        }

        public async Task SignOut(string name)
        {
            await Clients.AllExcept(Context.ConnectionId).SendAsync("online", $"{ name }leave the group.");
        }
    }
}