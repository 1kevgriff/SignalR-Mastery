
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

public class ColorHub : Hub
{
    public async Task JoinGroup(string groupName)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
    }

    public Task TriggerGroup(string groupName)
    {
        return Clients.Group(groupName).SendAsync("TriggerColor", groupName);
    }
}