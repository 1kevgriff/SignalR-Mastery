
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

public class ToDoHub : Hub
{
    private readonly IToDoRepository toDoRepository;

    public ToDoHub(IToDoRepository toDoRepository)
    {
        this.toDoRepository = toDoRepository;
    }

    public async Task GetLists()
    {
        var results = await toDoRepository.GetLists();

        await Clients.Caller.SendAsync("UpdatedToDoList", results);
    }

    public async Task GetList(int listId)
    {
        var result = await toDoRepository.GetList(listId);

        await Clients.Caller.SendAsync("UpdatedListData", result);
    }

    // SubscribeToCountUpdates
    public async Task SubscribeToCountUpdates()
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, "Counts");
    }

    // UnsubscribeToCountUpdates
    public async Task UnsubscribeFromCountUpdates()
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, "Counts");
    }

    // SubscribeToListUpdates
    public async Task SubscribeToListUpdates(int listId)
    {
        var groupName = ListIdToGroupName(listId);
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
    }

    // UnsubscribeToListUpdates
    public async Task UnsubscribeFromListUpdates(int listId)
    {
        var groupName = ListIdToGroupName(listId);
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
    }

    // AddToDoItem
    public async Task AddToDoItem(int listId, string text)
    {
        await toDoRepository.AddToDoItem(listId, text);

        // notify list count updates
        var allLists = await toDoRepository.GetLists();
        var listUpdate = await toDoRepository.GetList(listId);

        // notify list viewers on update
        var groupName = ListIdToGroupName(listId);
        await Clients.Group("Counts").SendAsync("UpdatedToDoList", allLists);
        await Clients.Group(groupName).SendAsync("UpdatedListData", listUpdate);
    }

    // ToggleToDoItem
    public async Task ToggleToDoItem(int listId, int itemId)
    {
        await toDoRepository.ToggleToDoItem(listId, itemId);

        // notify list count updates
        var allLists = await toDoRepository.GetLists();
        var listUpdate = await toDoRepository.GetList(listId);

        // notify list viewers on update
        var groupName = ListIdToGroupName(listId);
        await Clients.Group("Counts").SendAsync("UpdatedToDoList", allLists);
        await Clients.Group(groupName).SendAsync("UpdatedListData", listUpdate);
    }


    private string ListIdToGroupName(int listId) => $"list-updates-{listId}";
}