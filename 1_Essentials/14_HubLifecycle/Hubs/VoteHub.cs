
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

public class VoteHub : Hub
{
    private readonly IVoteManager voteManager;
    private readonly ILogger<VoteHub> logger;

    public VoteHub(IVoteManager voteManager, ILogger<VoteHub> logger)
    {
        this.voteManager = voteManager;
        this.logger = logger;

        logger.LogDebug($"VoteHub created. {DateTime.UtcNow.ToLongTimeString()}");
    }

    public Dictionary<string, int> GetCurrentVotes()
    {
        return voteManager.GetCurrentVotes();
    }
}