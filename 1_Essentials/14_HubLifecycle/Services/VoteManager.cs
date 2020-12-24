using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

public class VoteManager : IVoteManager
{
    private static Dictionary<string, int> votes;
    static VoteManager()
    {
        votes = new Dictionary<string, int>();
        votes.Add("pie", 0);
        votes.Add("bacon", 0);
    }

    public async Task CastVote(string voteFor)
    {
        votes[voteFor]++;
    }

    public Dictionary<string, int> GetCurrentVotes()
    {
        return votes;
    }
}