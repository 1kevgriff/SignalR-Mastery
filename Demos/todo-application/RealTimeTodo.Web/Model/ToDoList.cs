
using System;
using System.Collections.Generic;
using System.Linq;

public class ToDoList
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<ToDoItem> Items { get; set; }
    public int Pending => Items.Count(p => !p.IsCompleted);
    public int Completed => Items.Count(p => p.IsCompleted);

    public ToDoListMinimal GetMinimal()
    {
        return new ToDoListMinimal()
        {
            Id = Id,
            Name = Name,
            Pending = Pending,
            Completed = Completed
        };
    }

    public void AddItem(string text)
    {
        var newId = Items.Any() ? 
            Items.Max(p => p.Id) + 1 : 0;

        Items.Add(new ToDoItem()
        {
            Text = text,
            Id = newId
        });
    }

    public void Toggle(int itemId)
    {
        var item = Items.FirstOrDefault(p => p.Id.Equals(itemId));

        if (item == null) throw new NullReferenceException("Item not found.");

        item.IsCompleted = !item.IsCompleted;
    }
}
