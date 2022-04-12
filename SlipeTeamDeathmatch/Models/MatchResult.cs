using SlipeServer.Server.Elements;

namespace SlipeTeamDeathmatch.Models;

public struct MatchResult
{
    public Team? Winner { get; }
    public IEnumerable<Death> Deaths { get; }

    public MatchResult(Team? winner, IEnumerable<Death> deaths)
    {
        this.Winner = winner;
        this.Deaths = deaths;
    }
}

