
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

public class SyncHub : Hub
{
    #region Sync
    // public async Task SyncTextBox(string text)
    // {
    //     await Clients.Others.SendAsync("syncTextBox", text);
    // }

    // public async Task SyncCheckbox(bool checkbox)
    // {
    //     await Clients.Others.SendAsync("syncCheckbox", checkbox);
    // }
    #endregion

    #region Notify
    // public async Task StartNotify(){
    //     await Groups.AddToGroupAsync(Context.ConnectionId, "notify-me");
    // }
    // public async Task EndNotify() {
    //     await Groups.RemoveFromGroupAsync(Context.ConnectionId, "notify-me");
    // }
    #endregion
}