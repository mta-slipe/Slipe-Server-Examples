using SlipeServer.Server.Elements;

namespace SlipeTeamDeathmatch.Models;

public struct MatchResult
{
    public Team? Winner { get; }

    public MatchResult(Team? winner)
    {
        this.Winner = winner;
    }
}

