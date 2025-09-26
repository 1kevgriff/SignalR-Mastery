# SignalR Essentials Guide

## Overview
The Essentials section covers fundamental SignalR concepts required for building real-time applications.

## Topics Covered

### 1. Basic Client-Server Communication
**Location**: `1_Essentials/1_BasicClientServer/`
- Simple Hub implementation
- Client-server message exchange
- View counter example

### 2. Logging
**Location**: `1_Essentials/2_Logging/`
- Configuring SignalR logging
- Debug and diagnostic information
- Client and server-side logging

### 5. Transport Types
**Location**: `1_Essentials/5_ChoosingTransportType/`
- WebSockets (preferred)
- Server-Sent Events
- Long Polling
- Transport fallback strategies

### 7. Calling Hub Methods
**Location**: `1_Essentials/7_CallingHubMethods/`
- Invoking server methods from clients
- Method parameters and return values
- Async method patterns

### 8. Client Events
**Location**: `1_Essentials/8_ClientEvents/`
- Handling client-side events
- Connection lifecycle events
- Custom event handlers

### 11. Groups
**Location**: `1_Essentials/11_Groups/`
- Creating and managing groups
- Targeted messaging to group members
- Dynamic group membership
- ColorHub example implementation

### 12. Message Sizes
**Location**: `1_Essentials/12_MessageSizes/`
- Handling large payloads
- Message size limits configuration
- Streaming for large data
- RandomUser data example

### 13. Hub Context Outside Hub
**Location**: `1_Essentials/13_HubContextOutsideHub/`
- Using `IHubContext<T>` in controllers
- Sending messages from background services
- VoteController integration example

### 14. Hub Lifecycle
**Location**: `1_Essentials/14_HubLifecycle/`
- OnConnectedAsync/OnDisconnectedAsync
- Connection management
- User tracking

### 15. Reconnection
**Location**: `1_Essentials/15_Reconnection/`
- Automatic reconnection configuration
- Reconnection events
- Connection recovery strategies

### 16. Dependency Injection
**Location**: `1_Essentials/16_DependencyInjection/`
- Injecting services into Hubs
- Scoped vs Singleton services
- Service lifetime considerations

## Common Patterns

### Basic Hub Structure
```csharp
public class MyHub : Hub
{
    public async Task SendMessage(string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", message);
    }
}
```

### Client Connection
```typescript
const connection = new signalR.HubConnectionBuilder()
    .withUrl("/myHub")
    .build();

connection.on("ReceiveMessage", (message) => {
    console.log(message);
});

await connection.start();
```

### Startup Configuration
```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddSignalR();
}

public void Configure(IApplicationBuilder app)
{
    app.UseEndpoints(endpoints =>
    {
        endpoints.MapHub<MyHub>("/myHub");
    });
}
```