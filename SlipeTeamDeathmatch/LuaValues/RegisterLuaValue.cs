﻿using SlipeServer.Packets.Definitions.Lua;
using SlipeServer.SourceGenerators;

namespace SlipeTeamDeathmatch.LuaValues;

[LuaValue]
public partial class RegisterLuaValue
{
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string PasswordConfirmation { get; set; } = null!;

    public partial void Parse(LuaValue luaValue);
}