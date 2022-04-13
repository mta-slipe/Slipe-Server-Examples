using SlipeServer.Packets.Definitions.Lua;
using SlipeServer.Server.Elements;
using SlipeTeamDeathmatch.Models;

namespace SlipeTeamDeathmatch.Elements;

public class TdmPlayer : Player
{
    public Match? Match { get; set; }
    public Account Account { get; set; }

    public TdmPlayer() : base()
    {
        this.Account = new Account(Guid.NewGuid().ToString())
        {
            IsGuest = true
        };
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
