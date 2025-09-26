# SignalR Mastery - Project Overview

## About
SignalR Mastery is a comprehensive course repository demonstrating ASP.NET Core SignalR concepts from basics to advanced implementations. The project provides hands-on examples for building real-time web applications.

## Technology Stack

### Backend
- **.NET 6** (ASP.NET Core)
- **ASP.NET Core SignalR** - Real-time communication framework
- **Entity Framework Core** - Data access
- **ASP.NET Core Identity** - Authentication/Authorization

### Frontend
- **TypeScript** - Type-safe JavaScript
- **Webpack** - Module bundler
- **@microsoft/signalr** - SignalR JavaScript client
- **Node.js 12.x+** - Development environment

### Advanced Features
- **Azure SignalR Service** - Cloud scaling
- **Redis** (StackExchange.Redis) - Backplane for multi-server scenarios
- **MessagePack** - Binary serialization protocol

## Project Structure

```
SignalR-Mastery/
├── 1_Essentials/          # Basic SignalR concepts
├── 2_Advanced/            # Advanced features and patterns
├── Demos/                 # Complete application examples
├── Presentations/         # Course presentation materials
└── ref/                   # Reference documentation
```

## Quick Start

1. **Install Dependencies**
   ```bash
   npm run i
   ```

2. **Start Webpack**
   ```bash
   npm run build
   ```

3. **Run Application**
   ```bash
   dotnet run
   ```

## Key Learning Areas

- Real-time client-server communication
- Hub implementation and lifecycle
- Group management for targeted messaging
- Authentication and authorization
- Scaling with Azure SignalR Service
- Performance optimization with MessagePack
- Background services integration
- Multiple transport protocols (WebSockets, SSE, Long Polling)