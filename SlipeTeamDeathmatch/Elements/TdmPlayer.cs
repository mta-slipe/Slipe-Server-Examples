using SlipeServer.Packets.Definitions.Lua;
using SlipeServer.Packets.Lua.Event;
using SlipeServer.Server;
using SlipeServer.Server.Elements;
using SlipeServer.Server.Extensions;
using SlipeTeamDeathmatch.Models;

namespace SlipeTeamDeathmatch.Elements;

public class TdmPlayer : Player
{
    public Match? Match { get; set; }

    public TdmPlayer(Client client) : base(client)
    {
    }
}
