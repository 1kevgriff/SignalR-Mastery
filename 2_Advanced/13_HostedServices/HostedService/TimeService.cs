

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;

public class TimeService : IHostedService, IDisposable
{
    private readonly IHubContext<TimeHub> timeHub;
    private Timer _timer;

    public TimeService(IHubContext<TimeHub> timeHub)
    {
        this.timeHub = timeHub;
    }

    private void Tick(object state)
    {
        var currentTime = DateTime.UtcNow.ToString("F");

        timeHub.Clients.All.SendAsync("updateCurrentTime", currentTime);
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(Tick, null, 0, 500);

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();

    }
}