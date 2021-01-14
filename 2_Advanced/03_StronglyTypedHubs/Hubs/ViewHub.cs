
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

public class ViewHub : Hub<IHubClient>
{
    public static int ViewCount { get; set; } = 0;

    public async Task NotifyWatching()
    {
        ViewCount++;

        await this.Clients.All.ViewCountUpdate(ViewCount);
    }
}

public interface IHubClient 
{
    Task NewViewCount(int viewCount);
}