
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System.Linq;

public class UserHub : Hub
{
    private readonly IRandomUserService randomUserService;
    private readonly ILogger logger;

    public UserHub(IRandomUserService randomUserService, ILogger<UserHub> logger)
    {
        this.randomUserService = randomUserService;
        this.logger = logger;
    }
    public async Task<IEnumerable<RandomUser>> GetUsers(int max = 1)
    {
        var httpContext = Context.GetHttpContext();
        var headerToCheck = "X-Foo-Header";
        var headerValue = httpContext.Request.Headers[headerToCheck];
        
        if (headerValue.Any())
        {
            logger.LogInformation($"Header {headerToCheck} was found with value {headerValue}");
        }
        
        var randomUsers = await randomUserService.GetUsers(max);

        return randomUsers;
    }
}