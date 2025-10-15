using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace RealStateApp.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string room,string user, string message)
        {
            await Clients.Group(room).SendAsync("ReceiveMessage", user, message);
        }

        public async Task AddToGroup(string room)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, room);

            await Clients.Group(room).SendAsync("ShowWho", $"Se ha unido al chat {Context.ConnectionId}");
        }
    }
}

