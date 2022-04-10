using SlipeServer.Server.Elements;
using SlipeServer.Server.PacketHandling.Handlers.Middleware;
using SlipeTeamDeathmatch.Elements;

namespace SlipeTeamDeathmatch.Middleware;

public class MatchMiddleware<T> : ISyncHandlerMiddleware<T>
{
    public IEnumerable<Player> GetPlayersToSyncTo(Player player, T data)
    {
        return (player as TdmPlayer)!.Match?.Players.Where(x => x != player) ?? Array.Empty<TdmPlayer>();
    }
}
public class MatchWithSourceMiddleware<T> : ISyncHandlerMiddleware<T>
{
    public IEnumerable<Player> GetPlayersToSyncTo(Player player, T data)
    {
        return (player as TdmPlayer)!.Match?.Players ?? Array.Empty<TdmPlayer>();
    }
}

