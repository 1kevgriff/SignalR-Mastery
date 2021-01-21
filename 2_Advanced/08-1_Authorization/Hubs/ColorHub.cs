
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

public class ColorHub : Hub 
{
    [Authorize(Roles="ADMIN")]
    public Task ChangeBackground(string color){

        return Clients.All.SendAsync("changeBackground", color);
    }
}