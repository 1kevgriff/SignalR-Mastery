# SignalR Common Patterns and Best Practices

## Hub Patterns

### Basic Hub Implementation
```csharp
public class ChatHub : Hub
{
    public async Task SendMessage(string user, string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }
}
```

### Strongly Typed Hub
```csharp
public interface IChatClient
{
    Task ReceiveMessage(string user, string message);
    Task UserJoined(string user);
    Task UserLeft(string user);
}

public class TypedChatHub : Hub<IChatClient>
{
    public async Task SendMessage(string user, string message)
    {
        await Clients.All.ReceiveMessage(user, message);
    }
}
```

### Hub with Dependency Injection
```csharp
public class DataHub : Hub
{
    private readonly IDataService _dataService;
    private readonly ILogger<DataHub> _logger;

    public DataHub(IDataService dataService, ILogger<DataHub> logger)
    {
        _dataService = dataService;
        _logger = logger;
    }

    public async Task GetData(string id)
    {
        var data = await _dataService.GetByIdAsync(id);
        await Clients.Caller.SendAsync("DataReceived", data);
    }
}
```

## Connection Management

### Tracking Connected Users
```csharp
public class ConnectionHub : Hub
{
    private static readonly Dictionary<string, string> _connections = new();

    public override async Task OnConnectedAsync()
    {
        _connections[Context.ConnectionId] = Context.UserIdentifier ?? "Anonymous";
        await Clients.All.SendAsync("UserConnected", Context.ConnectionId);
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        _connections.Remove(Context.ConnectionId);
        await Clients.All.SendAsync("UserDisconnected", Context.ConnectionId);
        await base.OnDisconnectedAsync(exception);
    }
}
```

### Custom User Identifier Provider
```csharp
public class CustomUserIdProvider : IUserIdProvider
{
    public string GetUserId(HubConnectionContext connection)
    {
        return connection.User?.FindFirst("sub")?.Value
            ?? connection.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }
}

// Register in Startup
services.AddSingleton<IUserIdProvider, CustomUserIdProvider>();
```

## Group Management

### Dynamic Group Membership
```csharp
public class RoomHub : Hub
{
    public async Task JoinRoom(string roomName)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
        await Clients.Group(roomName).SendAsync("UserJoinedRoom", Context.UserIdentifier);
    }

    public async Task LeaveRoom(string roomName)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);
        await Clients.Group(roomName).SendAsync("UserLeftRoom", Context.UserIdentifier);
    }

    public async Task SendToRoom(string roomName, string message)
    {
        await Clients.Group(roomName).SendAsync("ReceiveRoomMessage", message);
    }
}
```

## Client Patterns

### TypeScript Client with Reconnection
```typescript
class SignalRConnection {
    private connection: signalR.HubConnection;

    constructor(url: string) {
        this.connection = new signalR.HubConnectionBuilder()
            .withUrl(url)
            .withAutomaticReconnect([0, 2000, 10000, 30000])
            .configureLogging(signalR.LogLevel.Information)
            .build();

        this.setupEventHandlers();
    }

    private setupEventHandlers(): void {
        this.connection.onreconnecting(() => {
            console.log("Reconnecting...");
        });

        this.connection.onreconnected(() => {
            console.log("Reconnected!");
        });

        this.connection.onclose(() => {
            console.log("Connection closed");
        });
    }

    async start(): Promise<void> {
        try {
            await this.connection.start();
            console.log("Connected!");
        } catch (err) {
            console.error("Error connecting:", err);
            setTimeout(() => this.start(), 5000);
        }
    }
}
```

### Message Queuing for Offline Support
```typescript
class QueuedConnection {
    private messageQueue: Array<() => Promise<void>> = [];
    private isConnected: boolean = false;

    async sendMessage(method: string, ...args: any[]): Promise<void> {
        const action = async () => {
            await this.connection.invoke(method, ...args);
        };

        if (this.isConnected) {
            await action();
        } else {
            this.messageQueue.push(action);
        }
    }

    private async processQueue(): Promise<void> {
        while (this.messageQueue.length > 0) {
            const action = this.messageQueue.shift();
            await action();
        }
    }
}
```

## Error Handling

### Hub Error Handling
```csharp
public class ErrorHandlingHub : Hub
{
    private readonly ILogger<ErrorHandlingHub> _logger;

    public async Task RiskyOperation(string input)
    {
        try
        {
            // Validate input
            if (string.IsNullOrEmpty(input))
            {
                throw new HubException("Invalid input provided");
            }

            // Perform operation
            await PerformOperation(input);
        }
        catch (HubException)
        {
            // HubExceptions are sent to the client
            throw;
        }
        catch (Exception ex)
        {
            // Log other exceptions but don't expose details
            _logger.LogError(ex, "Error in RiskyOperation");
            throw new HubException("An error occurred processing your request");
        }
    }
}
```

### Client Error Handling
```typescript
connection.invoke("RiskyOperation", input)
    .catch(err => {
        if (err.message.includes("Invalid input")) {
            showValidationError(err.message);
        } else {
            showGenericError("Operation failed");
            console.error(err);
        }
    });
```

## Performance Patterns

### Message Batching
```csharp
public class BatchingHub : Hub
{
    private readonly IMemoryCache _cache;
    private readonly Timer _flushTimer;

    public BatchingHub(IMemoryCache cache)
    {
        _cache = cache;
        _flushTimer = new Timer(FlushBatch, null, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1));
    }

    public async Task SendUpdate(UpdateData data)
    {
        var batch = _cache.GetOrCreate("batch", entry => new List<UpdateData>());
        batch.Add(data);

        if (batch.Count >= 100) // Flush if batch is large
        {
            await FlushBatch(null);
        }
    }

    private async void FlushBatch(object state)
    {
        var batch = _cache.Get<List<UpdateData>>("batch");
        if (batch?.Any() == true)
        {
            await Clients.All.SendAsync("BatchUpdate", batch);
            _cache.Remove("batch");
        }
    }
}
```

### Throttling
```typescript
function throttle(func: Function, delay: number) {
    let timeoutId: NodeJS.Timeout;
    let lastExecTime = 0;

    return function (...args: any[]) {
        const currentTime = Date.now();

        if (currentTime - lastExecTime > delay) {
            func.apply(this, args);
            lastExecTime = currentTime;
        } else {
            clearTimeout(timeoutId);
            timeoutId = setTimeout(() => {
                func.apply(this, args);
                lastExecTime = Date.now();
            }, delay - (currentTime - lastExecTime));
        }
    };
}

const throttledSend = throttle((data) => {
    connection.invoke("SendUpdate", data);
}, 100);
```

## Security Patterns

### JWT Authentication
```csharp
// Startup.cs
services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];
                var path = context.HttpContext.Request.Path;

                if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs"))
                {
                    context.Token = accessToken;
                }

                return Task.CompletedTask;
            }
        };
    });
```

### Authorization Policies
```csharp
services.AddAuthorization(options =>
{
    options.AddPolicy("HubAccess", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("hub_access", "true");
    });
});

[Authorize(Policy = "HubAccess")]
public class SecureHub : Hub { }
```