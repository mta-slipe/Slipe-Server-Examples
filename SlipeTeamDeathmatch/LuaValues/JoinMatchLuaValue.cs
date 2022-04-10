using SlipeServer.Packets.Definitions.Lua;
using SlipeServer.SourceGenerators;

namespace SlipeTeamDeathmatch.LuaValues;

[LuaValue]
public partial class JoinMatchLuaValue
{
    public int Id { get; set; }

    public partial void Parse(LuaValue luaValue);
}