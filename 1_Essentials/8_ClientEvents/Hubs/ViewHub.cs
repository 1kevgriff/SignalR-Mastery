
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

public class ViewHub : Hub
{
    private static int ViewCount {get;set;} = 0;

    public Task IncrementServerView()
    {
        ViewCount++;

        return Clients.All.SendAsync("incrementView", ViewCount);
    }
}