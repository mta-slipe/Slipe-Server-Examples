using SlipeServer.Packets.Definitions.Lua;
using SlipeServer.Server.Elements;
using SlipeServer.Server.Enums;
using SlipeTeamDeathmatch.Models;

namespace SlipeTeamDeathmatch.Elements;

public class TdmPlayer : Player
{
    public Match? Match { get; set; }
    public Account Account { get; set; }

    public TdmPlayer() : base()
    {
        this.Account = Account.CreateGuest();
    }

    public void SendErrorMessage(string message)
    {
        TriggerLuaEvent("Slipe.TeamDeathMatch.Error", this, message);
    }

    public void SendMatches(IEnumerable<Match> matches)
    {
        TriggerLuaEvent("Slipe.TeamDeathMatch.Matches", this, new LuaValue[] {
            new LuaValue(matches.Select(x => x.GetLuaValue()))
        });
    }

    public void SendMatch(Match match)
    {
        TriggerLuaEvent("Slipe.TeamDeathMatch.Match", this, match.GetLuaValue());
    }

    public void SendMaps(IEnumerable<string> maps)
    {
        TriggerLuaEvent("Slipe.TeamDeathMatch.Maps",
            parameters: new LuaValue(maps.Select(x => new LuaValue(x)).ToArray())
        );
    }

    public void SendStart()
    {
        TriggerLuaEvent("Slipe.TeamDeathMatch.Start");
    }

    public void SendLoggedIn()
    {
        TriggerLuaEvent("Slipe.TeamDeathMatch.LoggedIn");
    }

    public void SendLoggedOut()
    {
        TriggerLuaEvent("Slipe.TeamDeathMatch.LoggedOut");
    }

    public void ApplyMaxStats()
    {
        foreach (var weapon in Enum.GetValues<WeaponId>())
            this.SetWeaponStat(weapon, 1000);
    }

    public LuaValue GetLuaValue()
    {
        return new LuaValue(new Dictionary<LuaValue, LuaValue>()
        {
            ["element"] = this.Id,
            ["name"] = this.Name,
            ["matchCount"] = this.Account.MatchCount,
            ["killCount"] = this.Account.KillCount,
            ["deathCount"] = this.Account.DeathCount
        });
    }
}
