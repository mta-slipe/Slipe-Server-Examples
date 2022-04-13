using SlipeServer.Server.Events;
using SlipeServer.Server.Services;
using SlipeTeamDeathmatch.Elements;
using SlipeTeamDeathmatch.LuaValues;
using SlipeTeamDeathmatch.Services;

namespace SlipeTeamDeathmatch.Logic;

public class MatchLogic
{
    private readonly LuaEventService luaEventService;
    private readonly MapService mapService;
    private readonly MatchService matchService;

    public MatchLogic(
        LuaEventService luaEventService,
        MapService mapProvider,
        MatchService matchService)
    {
        this.luaEventService = luaEventService;
        this.mapService = mapProvider;
        this.matchService = matchService;

        this.luaEventService.AddEventHandler("Slipe.TeamDeathMatch.RequestMatches", HandleRequestMatches);
        this.luaEventService.AddEventHandler("Slipe.TeamDeathMatch.RequestMaps", HandleRequestMaps);
        this.luaEventService.AddEventHandler("Slipe.TeamDeathMatch.CreateMatch", HandleCreateRequest);
        this.luaEventService.AddEventHandler("Slipe.TeamDeathMatch.JoinMatch", HandleJoinRequest);
        this.luaEventService.AddEventHandler("Slipe.TeamDeathMatch.LeaveMatch", HandleLeaveRequest);
        this.luaEventService.AddEventHandler("Slipe.TeamDeathMatch.SetMap", HandleSetMapRequest);
        this.luaEventService.AddEventHandler("Slipe.TeamDeathMatch.StartMatch", HandleStartMatchRequest);
    }

    private void HandleRequestMatches(LuaEvent luaEvent)
    {
        (luaEvent.Player as TdmPlayer)!.SendMatches(this.matchService.Matches);
    }

    private void HandleRequestMaps(LuaEvent luaEvent)
    {
        (luaEvent.Player as TdmPlayer)!.SendMaps(this.mapService.AvailableMaps);
    }

    private void HandleCreateRequest(LuaEvent luaEvent)
    {
        var player = (luaEvent.Player as TdmPlayer)!;
        var luaValue = new CreateMatchLuaValue();
        luaValue.Parse(luaEvent.Parameters[0]);

        this.matchService.CreateMatch(player, luaValue.Name);
    }

    private void HandleJoinRequest(LuaEvent luaEvent)
    {
        var player = (luaEvent.Player as TdmPlayer)!;
        var luaValue = new JoinMatchLuaValue();
        luaValue.Parse(luaEvent.Parameters[0]);

        var match = this.matchService.Get(luaValue.Id);
        this.matchService.AddPlayerToMatch(player, match);
    }

    private void HandleLeaveRequest(LuaEvent luaEvent)
    {
        var player = (luaEvent.Player as TdmPlayer)!;

        this.matchService.RemovePlayerFromMatch(player);
    }

    private void HandleStartMatchRequest(LuaEvent luaEvent)
    {
        var player = (luaEvent.Player as TdmPlayer)!;

        if (player.Match != null)
            this.matchService.StartMatch(player, player.Match);
    }

    private void HandleSetMapRequest(LuaEvent luaEvent)
    {
        var player = (luaEvent.Player as TdmPlayer)!;
        var luaValue = new SelectMapLuaValue();
        luaValue.Parse(luaEvent.Parameters[0]);

        if (player.Match != null)
            this.matchService.SetMatchMap(player, player.Match, luaValue.Name);
    }
}
