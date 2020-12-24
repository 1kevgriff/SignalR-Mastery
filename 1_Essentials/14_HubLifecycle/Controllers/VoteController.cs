
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

public class VoteController : ControllerBase
{
    private readonly IVoteManager voteManager;
        private readonly IHubContext<VoteHub> hubContext;

    public VoteController(IVoteManager voteManager, IHubContext<VoteHub> hubContext)
    {
        this.voteManager = voteManager;
        this.hubContext = hubContext;
    }

    [HttpPost("/vote/pie")]
    public async Task<IActionResult> VotePie()
    {
        // save vote
        await voteManager.CastVote("pie");

        await hubContext.Clients.All.SendAsync("updateVotes", voteManager.GetCurrentVotes());

        return Ok();
    }

    [HttpPost("/vote/bacon")]
    public async Task<IActionResult> VoteBacon()
    {
        // save vote
        await voteManager.CastVote("bacon");

        await hubContext.Clients.All.SendAsync("updateVotes", voteManager.GetCurrentVotes());

        return Ok();
    }
}