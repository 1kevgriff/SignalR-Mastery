# Demo Applications Guide

## Real-Time Todo Application

**Location**: `Demos/todo-application/`

### Overview
A complete Single Page Application demonstrating real-time collaboration features using SignalR.

### Key Features
- Real-time todo item synchronization
- Multiple user collaboration
- Group-based updates
- Repository pattern implementation
- Dependency injection

### Architecture

#### Backend Components

**ToDoHub** (`Hubs/ToDoHub.cs`)
- Central Hub for todo operations
- Group management for targeted updates
- Dependency injection of repository

**Repository Pattern**
- `IToDoRepository` interface
- In-memory implementation
- CRUD operations for todo items

**Models**
- Todo item model
- Data transfer objects (DTOs)

#### Frontend Components

**TypeScript Client**
- SignalR connection management
- Real-time UI updates
- Event handling
- State management

### Key Implementations

#### Hub with Dependency Injection
```csharp
public class ToDoHub : Hub
{
    private readonly IToDoRepository _repository;

    public ToDoHub(IToDoRepository repository)
    {
        _repository = repository;
    }

    public async Task AddTodo(TodoItem item)
    {
        var added = _repository.Add(item);
        await Clients.Others.SendAsync("TodoAdded", added);
    }
}
```

#### Group Management
```csharp
public async Task JoinTodoList(string listId)
{
    await Groups.AddToGroupAsync(Context.ConnectionId, $"list-{listId}");
    var todos = _repository.GetByList(listId);
    await Clients.Caller.SendAsync("LoadTodos", todos);
}
```

#### Client Implementation
```typescript
// Connection setup
const connection = new signalR.HubConnectionBuilder()
    .withUrl("/todoHub")
    .withAutomaticReconnect()
    .build();

// Event handlers
connection.on("TodoAdded", (todo) => {
    addTodoToUI(todo);
});

connection.on("TodoUpdated", (todo) => {
    updateTodoInUI(todo);
});

connection.on("TodoDeleted", (id) => {
    removeTodoFromUI(id);
});

// Operations
async function addTodo(title: string) {
    await connection.invoke("AddTodo", { title, completed: false });
}
```

### Running the Demo

1. **Navigate to Demo Directory**
   ```bash
   cd Demos/todo-application
   ```

2. **Install Dependencies**
   ```bash
   npm run i
   ```

3. **Build Frontend**
   ```bash
   npm run build
   ```

4. **Run Application**
   ```bash
   dotnet run
   ```

5. **Access Application**
   - Open browser to `https://localhost:5001`
   - Open multiple tabs to see real-time sync

### Learning Points

#### Real-Time Patterns
- Optimistic UI updates
- Conflict resolution
- State synchronization
- Connection recovery

#### SignalR Features Demonstrated
- Hub method invocation
- Client callbacks
- Group messaging
- Connection lifecycle
- Automatic reconnection

#### Best Practices
- Repository pattern for data access
- Dependency injection
- Separation of concerns
- Error handling
- TypeScript for type safety

### Extension Ideas

1. **Authentication**
   - User-specific todo lists
   - Role-based permissions

2. **Persistence**
   - Entity Framework Core integration
   - Database storage

3. **Advanced Features**
   - Collaborative editing
   - Presence indicators
   - Typing indicators
   - Conflict resolution

4. **Performance**
   - Message batching
   - Debouncing updates
   - Caching strategies

### Common Issues and Solutions

**Issue**: Changes not syncing
- Check connection status
- Verify Hub method names match
- Ensure proper group membership

**Issue**: Performance degradation
- Implement message throttling
- Use MessagePack protocol
- Consider Azure SignalR Service

**Issue**: Connection drops
- Configure automatic reconnection
- Implement connection state UI
- Add retry logic