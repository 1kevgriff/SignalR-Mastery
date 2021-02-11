
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

public class TimeHub : Hub
{
    public async Task GetCurrentTime()
    {
        await Clients.Caller.SendAsync("UpdatedTime", DateTime.UtcNow.ToString("F"));
    }
}