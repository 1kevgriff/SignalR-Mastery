# SignalR Troubleshooting Guide

## Common Issues and Solutions

### Connection Issues

#### Problem: Connection fails to establish
**Symptoms**
- Client shows "Error: Failed to complete negotiation"
- Connection timeout errors
- 404 errors when accessing Hub endpoint

**Solutions**
1. Verify Hub routing configuration:
```csharp
app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<MyHub>("/myHub"); // Ensure path matches client
});
```

2. Check CORS configuration:
```csharp
services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://localhost:3000")
               .AllowAnyHeader()
               .AllowAnyMethod()
               .AllowCredentials(); // Required for SignalR
    });
});

app.UseCors(); // Must be before UseEndpoints
```

3. Verify client URL:
```typescript
const connection = new signalR.HubConnectionBuilder()
    .withUrl("https://localhost:5001/myHub") // Full URL or relative path
    .build();
```

#### Problem: Frequent disconnections
**Solutions**
- Configure keep-alive settings:
```csharp
services.AddSignalR(options =>
{
    options.KeepAliveInterval = TimeSpan.FromSeconds(15);
    options.ClientTimeoutInterval = TimeSpan.FromSeconds(30);
});
```

- Implement automatic reconnection:
```typescript
const connection = new signalR.HubConnectionBuilder()
    .withUrl("/hub")
    .withAutomaticReconnect([0, 2000, 10000, 30000])
    .build();
```

### Transport Issues

#### Problem: WebSocket connection fails
**Symptoms**
- Falls back to Server-Sent Events or Long Polling
- Performance degradation

**Solutions**
1. Enable WebSockets in IIS:
```xml
<system.webServer>
    <webSocket enabled="true" />
</system.webServer>
```

2. Check proxy/reverse proxy configuration (nginx):
```nginx
location /myHub {
    proxy_pass http://localhost:5000;
    proxy_http_version 1.1;
    proxy_set_header Upgrade $http_upgrade;
    proxy_set_header Connection "upgrade";
    proxy_set_header Host $host;
    proxy_cache_bypass $http_upgrade;
}
```

3. Force specific transport:
```typescript
const connection = new signalR.HubConnectionBuilder()
    .withUrl("/hub", {
        transport: signalR.HttpTransportType.WebSockets
    })
    .build();
```

### Authentication Issues

#### Problem: 401 Unauthorized errors
**Solutions**
1. Configure authentication for SignalR:
```csharp
services.AddAuthentication()
    .AddJwtBearer(options =>
    {
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];
                if (!string.IsNullOrEmpty(accessToken))
                {
                    context.Token = accessToken;
                }
                return Task.CompletedTask;
            }
        };
    });
```

2. Pass token from client:
```typescript
const connection = new signalR.HubConnectionBuilder()
    .withUrl("/hub", {
        accessTokenFactory: () => getAccessToken()
    })
    .build();
```

### Message Size Issues

#### Problem: "Connection disconnected with error 'Error: Server returned an error on close: Connection closed with an error.'"
**Solutions**
1. Increase message size limits:
```csharp
services.AddSignalR(options =>
{
    options.MaximumReceiveMessageSize = 102400; // 100 KB
});
```

2. Implement message chunking:
```csharp
public async Task SendLargeData(string id)
{
    var data = GetLargeData(id);
    var chunks = data.Chunk(1000); // Split into chunks

    foreach (var chunk in chunks)
    {
        await Clients.Caller.SendAsync("ReceiveChunk", chunk);
    }

    await Clients.Caller.SendAsync("ReceiveComplete");
}
```

### Performance Issues

#### Problem: High CPU/Memory usage
**Solutions**
1. Use MessagePack protocol:
```csharp
services.AddSignalR()
    .AddMessagePackProtocol();
```

2. Implement connection limits:
```csharp
services.AddSignalR(options =>
{
    options.MaximumParallelInvocationsPerClient = 1;
});
```

3. Use groups efficiently:
```csharp
// Good: Use groups for targeting
await Clients.Group("room1").SendAsync("Message", data);

// Avoid: Iterating through connections
foreach (var connectionId in connections)
{
    await Clients.Client(connectionId).SendAsync("Message", data);
}
```

### Scaling Issues

#### Problem: Messages not received across multiple servers
**Solutions**
1. Configure Redis backplane:
```csharp
services.AddSignalR()
    .AddStackExchangeRedis("localhost:6379", options =>
    {
        options.Configuration.ChannelPrefix = "MyApp";
    });
```

2. Use Azure SignalR Service:
```csharp
services.AddSignalR()
    .AddAzureSignalR(Configuration["Azure:SignalR:ConnectionString"]);
```

## Debugging Techniques

### Enable Detailed Logging

#### Server-side logging:
```csharp
public class Startup
{
    public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
    {
        loggerFactory.AddConsole(LogLevel.Debug);

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapHub<MyHub>("/hub");
        });
    }
}
```

#### Client-side logging:
```typescript
const connection = new signalR.HubConnectionBuilder()
    .withUrl("/hub")
    .configureLogging(signalR.LogLevel.Debug)
    .build();
```

### Browser Developer Tools

1. **Network Tab**
   - Check negotiation requests
   - Verify WebSocket connections
   - Monitor message payloads

2. **Console**
   - Enable SignalR logging
   - Check for JavaScript errors

### Diagnostic Tools

#### SignalR Performance Counters
```csharp
services.AddSignalR()
    .AddHubOptions<MyHub>(options =>
    {
        options.EnableDetailedErrors = true; // Development only
    });
```

#### Custom Diagnostics Hub
```csharp
public class DiagnosticsHub : Hub
{
    private readonly IHubContext<MyHub> _hubContext;

    public async Task GetConnectionCount()
    {
        var count = _hubContext.Clients.All.ToString(); // Example
        await Clients.Caller.SendAsync("ConnectionCount", count);
    }

    public async Task Ping()
    {
        await Clients.Caller.SendAsync("Pong", DateTime.UtcNow);
    }
}
```

## Testing Strategies

### Unit Testing Hubs
```csharp
[TestClass]
public class HubTests
{
    [TestMethod]
    public async Task SendMessage_BroadcastsToAllClients()
    {
        var mockClients = new Mock<IHubCallerClients>();
        var mockClientProxy = new Mock<IClientProxy>();

        mockClients.Setup(x => x.All).Returns(mockClientProxy.Object);

        var hub = new ChatHub
        {
            Clients = mockClients.Object
        };

        await hub.SendMessage("test", "message");

        mockClientProxy.Verify(x => x.SendCoreAsync(
            "ReceiveMessage",
            It.Is<object[]>(o => o.Length == 2),
            default), Times.Once);
    }
}
```

### Load Testing
```csharp
// Using SignalR client for load testing
var tasks = new List<Task>();

for (int i = 0; i < 1000; i++)
{
    tasks.Add(Task.Run(async () =>
    {
        var connection = new HubConnectionBuilder()
            .WithUrl("https://localhost:5001/hub")
            .Build();

        await connection.StartAsync();
        await connection.InvokeAsync("SendMessage", "Load test");
        await connection.DisposeAsync();
    }));
}

await Task.WhenAll(tasks);
```