using SlipeServer.Packets.Definitions.Lua;
using SlipeServer.Server.Enums;
using SlipeTeamDeathmatch.Elements;

namespace SlipeTeamDeathmatch.Models;

public struct Death
{
    public TdmPlayer Victim { get; }
    public TdmPlayer? Killer { get; }
    public WeaponId Weapon { get; }

    public Death(TdmPlayer victim, TdmPlayer? killer, WeaponId weapon)
    {
        this.Victim = victim;
        this.Killer = killer;
        this.Weapon = weapon;
    }

    public LuaValue GetLuaValue()
    {
        return new LuaValue(new Dictionary<LuaValue, LuaValue>()
        {
            ["killer"] = this.Killer?.Id ?? new LuaValue(),
            ["victim"] = this.Victim.Id,
            ["weapon"] = this.Weapon.ToString()
        });
    }
}

