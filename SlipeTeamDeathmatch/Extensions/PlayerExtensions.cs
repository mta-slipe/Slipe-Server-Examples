using SlipeServer.Packets.Definitions.Lua;
using SlipeServer.Packets.Lua.Event;
using SlipeServer.Server.Elements;
using SlipeServer.Server.Extensions;

namespace SlipeTeamDeathmatch.Extensions;
public static class PlayerExtensions
{
    public static void SendErrorMessage(this Player player, string message)
    {
        new LuaEventPacket("Slipe.TeamDeathMatch.Error", player.Id, new LuaValue[] { message })
            .SendTo(player);
    }
}
