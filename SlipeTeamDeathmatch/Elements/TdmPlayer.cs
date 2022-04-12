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
    public Account Account { get; set; }

    public TdmPlayer(Client client) : base(client)
    {
        this.Account = new Account(Guid.NewGuid().ToString())
        {
            IsGuest = true
        };
    }

    public void SendErrorMessage(string message)
    {
        TriggerEvent("Slipe.TeamDeathMatch.Error", message);
    }

    public void SendMatches(IEnumerable<Match> matches)
    {
        TriggerEvent("Slipe.TeamDeathMatch.Matches", new LuaValue[] {
            new LuaValue(matches.Select(x => x.GetLuaValue()))
        });
    }

    public void SendMatch(Match match)
    {
        TriggerEvent("Slipe.TeamDeathMatch.Match", match.GetLuaValue());
    }

    public void SendMaps(IEnumerable<string> maps)
    {
        TriggerEvent("Slipe.TeamDeathMatch.Maps",
            new LuaValue(maps.Select(x => new LuaValue(x)).ToArray())
        );
    }

    public void SendStart()
    {
        TriggerEvent("Slipe.TeamDeathMatch.Start");
    }

    private void TriggerEvent(string eventName, params LuaValue[] values)
    {
        new LuaEventPacket(eventName, this.Id, values)
            .SendTo(this);
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
