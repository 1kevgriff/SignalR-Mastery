using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

public interface IVoteManager
{
    Task CastVote(string voteFor);
    Dictionary<string, int> GetCurrentVotes();
}
