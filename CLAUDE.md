# SignalR Mastery - AI Assistant Guide

## Project Overview
This is a comprehensive SignalR course repository containing examples from basic to advanced real-time web application development using ASP.NET Core SignalR.

## Quick Reference

### Documentation
- **[Project Overview](ref/project-overview.md)** - Technology stack and structure
- **[Essentials Guide](ref/essentials-guide.md)** - Basic SignalR concepts and examples
- **[Advanced Features](ref/advanced-features.md)** - Production-ready patterns and scaling
- **[Demo Applications](ref/demo-applications.md)** - Complete application examples
- **[Common Patterns](ref/common-patterns.md)** - Best practices and code patterns
- **[Troubleshooting](ref/troubleshooting.md)** - Common issues and solutions

### Key Directories
- `1_Essentials/` - Fundamental SignalR concepts (16 examples)
- `2_Advanced/` - Advanced features and enterprise patterns (11 examples)
- `Demos/todo-application/` - Complete real-time todo application

## Development Commands

### Standard Workflow
```bash
# Install dependencies
npm run i

# Start Webpack (watch mode)
npm run build

# Run application
dotnet run
```

### Utilities
- `Clean.ps1` - Clean build artifacts
- `Upgrade.ps1` - Upgrade dependencies

## Technology Stack

### Backend
- .NET 6 (ASP.NET Core)
- ASP.NET Core SignalR
- Entity Framework Core
- Azure SignalR Service
- Redis (StackExchange.Redis)

### Frontend
- TypeScript
- Webpack
- @microsoft/signalr client library

## Common Tasks

### Create a New Hub
```csharp
public class MyHub : Hub
{
    public async Task SendMessage(string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", message);
    }
}
```

### Configure SignalR
```csharp
// Startup.cs
services.AddSignalR();

app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<MyHub>("/myHub");
});
```

### Client Connection
```typescript
const connection = new signalR.HubConnectionBuilder()
    .withUrl("/myHub")
    .withAutomaticReconnect()
    .build();

await connection.start();
```

## Project Structure Pattern

Each example follows a consistent structure:
- `Startup.cs` - SignalR configuration
- `Hubs/` - Hub implementations
- `wwwroot/` - Static files
- `package.json` - NPM dependencies
- `tsconfig.json` - TypeScript config
- `webpack.config.js` - Build configuration

## Key Concepts Covered

### Essentials
- Client-Server communication
- Transport protocols (WebSockets, SSE, Long Polling)
- Groups and targeted messaging
- Dependency injection
- Connection lifecycle

### Advanced
- MessagePack protocol
- Strongly typed Hubs
- Authorization/Authentication
- Azure SignalR Service
- Redis backplane for scaling
- Background services integration

## Testing & Debugging

### Enable Detailed Logging
```csharp
services.AddSignalR(options =>
{
    options.EnableDetailedErrors = true; // Dev only
});
```

### Client Logging
```typescript
.configureLogging(signalR.LogLevel.Debug)
```

## Common Issues

1. **Connection fails**: Check CORS, routing, and URL configuration
2. **WebSocket fallback**: Verify WebSocket support in hosting environment
3. **Authentication errors**: Configure JWT token handling for SignalR
4. **Scaling issues**: Implement Redis backplane or Azure SignalR Service

## Important Notes

- All examples use .NET 6 (upgraded from .NET 5)
- Automatic dependency updates via Renovate
- Examples demonstrate progression from simple to complex scenarios
- Each example is self-contained and runnable independently

## Course Information
- Course website: https://signalrmastery.com
- Support: Available through course community

## Development Tips

1. Start with examples in `1_Essentials/` to understand basics
2. Move to `2_Advanced/` for production patterns
3. Study `Demos/todo-application/` for complete implementation
4. Use TypeScript for type safety in client code
5. Configure automatic reconnection for production
6. Consider MessagePack for performance optimization
7. Use groups for efficient targeted messaging
8. Implement proper error handling and logging