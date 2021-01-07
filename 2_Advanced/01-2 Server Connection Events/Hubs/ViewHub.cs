
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

public class ViewHub : Hub
{
    public static int ViewCount { get; set; } = 0;

    public async override Task OnConnectedAsync()
    {
        ViewCount++;

        await this.Clients.All.SendAsync("viewCountUpdate", ViewCount);

        await base.OnConnectedAsync();
    }
    public async override Task OnDisconnectedAsync(Exception exception)
    {
        ViewCount--;

        await this.Clients.All.SendAsync("viewCountUpdate", ViewCount);

        await base.OnDisconnectedAsync(exception);
    }
}