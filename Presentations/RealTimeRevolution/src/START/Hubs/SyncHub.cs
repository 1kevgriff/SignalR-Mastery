using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace src.Hubs
{
    public class SyncHub : Hub
    {
        public async Task SyncTextBox(string text)
        {
            await Clients.Others.SendAsync("syncTextBox", text);
        }

        public async Task SyncCheckbox(bool checkbox)
        {
            await Clients.Others.SendAsync("syncCheckbox", checkbox);
        }

        public async Task StartNotify()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "notify-me");
        }
        public async Task EndNotify()
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "notify-me");
        }
    }
}
