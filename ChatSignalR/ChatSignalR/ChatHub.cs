using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace ChatSignalR
{
    [Authorize]
    public class ChatHub : Hub
    {
        public async Task Send(string message, string username)
        {
            await Clients.All.SendAsync("Send", message, username);
        }
    }
}
