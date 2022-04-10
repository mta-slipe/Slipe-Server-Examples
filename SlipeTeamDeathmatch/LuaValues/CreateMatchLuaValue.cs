using SlipeServer.Packets.Definitions.Lua;
using SlipeServer.SourceGenerators;

namespace SlipeTeamDeathmatch.LuaValues;

[LuaValue]
public partial class CreateMatchLuaValue
{
    public string Name { get; set; } = null!;

    public partial void Parse(LuaValue luaValue);
}