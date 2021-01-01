using System.Collections.Generic;
using System.Threading.Tasks;

public interface IToDoRepository
{
    Task<IEnumerable<ToDoListMinimal>> GetLists();
    Task<ToDoList> GetList(int id);

    Task AddToDoItem(int listId, string text);
    Task ToggleToDoItem(int listId, int itemId);
}
