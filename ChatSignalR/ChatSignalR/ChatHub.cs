using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using ChatSignalR.Controllers;
using ChatSignalR.Data;
using ChatSignalR.Models;

namespace ChatSignalR
{
    [Authorize]
    public class ChatHub : Hub
    {
        public async Task Send(string message, string userName, string roomName)
        {
            await Clients.Group(roomName).SendAsync("Receive", message, userName);
        }

        [Authorize(Roles = "admin")]
        public async Task Notify(string message, string userName)
        {
            await Clients.All.SendAsync("Receive", message, userName);
        }

        public Task JoinRoom(string roomName)
        {
            Groups.AddToGroupAsync(Context.ConnectionId, roomName);
            return Clients.Group(roomName).SendAsync("Receive", Context.User.Identity.Name + " joined.", "System");
        }

        public Task LeaveRoom(string roomName)
        {
            Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);
            return Clients.Group(roomName).SendAsync("Receive", Context.User.Identity.Name + " left.", "System");
        }
        
    }
}