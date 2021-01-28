
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

public class BackgroundColorHub : Hub
{
    public async Task ChangeBackground(string color){
        await this.Clients.All.SendAsync("changeBackground", color);
    }
}