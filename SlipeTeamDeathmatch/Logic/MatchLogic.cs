using Microsoft.Extensions.Logging;
using SlipeServer.Server;
using SlipeServer.Server.Elements;
using SlipeServer.Server.Elements.Events;
using SlipeServer.Server.Events;
using SlipeServer.Server.Resources;
using SlipeServer.Server.Resources.Providers;
using SlipeServer.Server.Services;
using SlipeTeamDeathmatch.Elements;
using SlipeTeamDeathmatch.LuaValues;
using SlipeTeamDeathmatch.Models;
using SlipeTeamDeathmatch.Services;

namespace SlipeTeamDeathmatch.Logic;

public class MatchLogic
{
    private int matchId;
    private readonly List<Match> matches;
    private readonly LuaEventService luaEventService;
    private readonly ILogger logger;
    private readonly MapService mapService;
    private Resource resource;

    public MatchLogic(MtaServer mtaServer,
        LuaEventService luaEventService,
        ILogger logger,
        MapService mapProvider,
        IResourceProvider resourceProvider,
        CommandService commandService)
    {
        this.luaEventService = luaEventService;
        this.logger = logger;
        this.mapService = mapProvider;

        this.matches = new();

        this.resource = resourceProvider.GetResource("SlipeTdm");
        commandService.AddCommand("restart").Triggered += (s, e) =>
        {
            this.resource.Stop();
            this.resource = resourceProvider.GetResource("SlipeTdm");
            this.resource.Start();
        };

        mtaServer.PlayerJoined += HandlePlayerJoin;

        this.luaEventService.AddEventHandler("Slipe.TeamDeathMatch.RequestMatches", HandleRequestMatches);
        this.luaEventService.AddEventHandler("Slipe.TeamDeathMatch.RequestMaps", HandleRequestMaps);
        this.luaEventService.AddEventHandler("Slipe.TeamDeathMatch.JoinMatch", HandleJoinRequest);
        this.luaEventService.AddEventHandler("Slipe.TeamDeathMatch.LeaveMatch", HandleLeaveRequest);
        this.luaEventService.AddEventHandler("Slipe.TeamDeathMatch.CreateMatch", HandleCreateRequest);
        this.luaEventService.AddEventHandler("Slipe.TeamDeathMatch.SetMap", HandleSetMapRequest);
        this.luaEventService.AddEventHandler("Slipe.TeamDeathMatch.StartMatch", HandleStartMatchRequest);
    }

    private void HandlePlayerJoin(Player player)
    {
        this.resource.StartFor(player);

        player.ResourceStarted += HandlePlayerResourceStarted;
    }

    private void HandlePlayerResourceStarted(Player sender, PlayerResourceStartedEventArgs e)
    {
        var player = (sender as TdmPlayer)!;
        if (e.NetId == this.resource.NetId)
        {
            if (player.Match == null)
                player.SendMatches(this.matches);
            else
                player.SendMatch(player.Match);
        }
    }

    private void HandleRequestMatches(LuaEvent luaEvent)
    {
        (luaEvent.Player as TdmPlayer)!.SendMatches(this.matches);
    }

    private void HandleRequestMaps(LuaEvent luaEvent)
    {
        (luaEvent.Player as TdmPlayer)!.SendMaps(this.mapService.AvailableMaps);
    }

    private void HandleJoinRequest(LuaEvent luaEvent)
    {
        var player = (luaEvent.Player as TdmPlayer)!;
        if (player.Match != null)
        {
            player.SendErrorMessage("You can not join this match as you are already in a match.");
            return;
        }
        var luaValue = new JoinMatchLuaValue();
        luaValue.Parse(luaEvent.Parameters[0]);

        var match = this.matches.Single(x => x.Id == luaValue.Id);
        match.AddPlayer(player);

        this.logger.LogInformation("{player} joined match \"{match}\" ({id})", player.Name, match.Name, match.Id);
    }

    private void HandleLeaveRequest(LuaEvent luaEvent)
    {
        var player = (luaEvent.Player as TdmPlayer)!;
        var match = player.Match;

        if (match == null)
            return;

        this.logger.LogInformation("{player} left match \"{match}\" ({id})", player.Name, match.Name, match.Id);

        match.RemovePlayer(player);
        player.SendMatches(this.matches);
    }

    private void HandleCreateRequest(LuaEvent luaEvent)
    {
        var player = (luaEvent.Player as TdmPlayer)!;
        if (player.Match != null)
        {
            player.SendErrorMessage("You can not create a match because you are already in one.");
            return;
        }
        var luaValue = new CreateMatchLuaValue();
        luaValue.Parse(luaEvent.Parameters[0]);

        if (string.IsNullOrEmpty(luaValue.Name.Trim()))
        {
            player.SendErrorMessage("You must enter a name for your match.");
            return;
        }

        var match = new Match(this.matchId++, luaValue.Name, player);
        this.matches.Add(match);
        match.Abandoned += (sender, args) =>
        {
            this.matches.Remove(match);
            this.logger.LogInformation("Match \"{match}\" was abandoned", match.Name);
        };

        player.SendMatch(match);

        this.logger.LogInformation("{player} created match \"{match}\" ({id})", player.Name, match.Name, match.Id);
    }

    private void HandleStartMatchRequest(LuaEvent luaEvent)
    {
        var player = (luaEvent.Player as TdmPlayer)!;
        var match = player.Match;
        if (match != null && match.Host == player && match.CanStart)
        {
            match.Start();
            this.logger.LogInformation("{player} started the match \"{match}\" ({id})", player.Name, match.Name, match.Id);
        }
        else
            player.SendErrorMessage("You can not start this match");
    }

    private void HandleSetMapRequest(LuaEvent luaEvent)
    {
        var player = (luaEvent.Player as TdmPlayer)!;
        var luaValue = new SelectMapLuaValue();
        luaValue.Parse(luaEvent.Parameters[0]);

        var map = this.mapService.GetMap(luaValue.Name);


        if (player.Match != null && player.Match.Host == player)
        {
            player.Match.SetMap(map);
            this.logger.LogInformation("{player} set the map to \"{map}\" for match \"{match}\" ({id})", player.Name, luaValue.Name, player.Match.Name, player.Match.Id);

            foreach (var matchPlayer in player.Match.Players)
                matchPlayer.SendMatch(player.Match);
        }
        else
            player.SendErrorMessage("You can not set the map for this match");
    }
}
