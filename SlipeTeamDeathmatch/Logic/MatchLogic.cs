using Microsoft.Extensions.Logging;
using SlipeServer.Packets.Definitions.Lua;
using SlipeServer.Server;
using SlipeServer.Server.Elements;
using SlipeServer.Server.Events;
using SlipeServer.Server.Resources;
using SlipeServer.Server.Resources.Providers;
using SlipeServer.Server.Services;
using SlipeTeamDeathmatch.Elements;
using SlipeTeamDeathmatch.Extensions;
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
    }

    private void HandleRequestMatches(LuaEvent luaEvent)
    {
        this.luaEventService.TriggerEvent("Slipe.TeamDeathMatch.Matches", luaEvent.Player, GetMatchesLuaValue());
    }

    private void HandleRequestMaps(LuaEvent luaEvent)
    {
        this.luaEventService.TriggerEvent("Slipe.TeamDeathMatch.Maps", luaEvent.Player, GetMapsLuaValue());
    }

    private void HandleJoinRequest(LuaEvent luaEvent)
    {
        try
        {
            var luaValue = new JoinMatchLuaValue();
            luaValue.Parse(luaEvent.Parameters[0]);

            var player = (luaEvent.Player as TdmPlayer)!;

            var match = this.matches.Single(x => x.Id == luaValue.Id);
            match.AddPlayer(player);

            this.logger.LogInformation("{player} joined match \"{match}\" ({id})", player.Name, match.Name, match.Id);
        }
        catch (Exception e)
        {
            luaEvent.Player.SendErrorMessage("Unable to join game");
            this.logger.LogError("{message}", e.Message);
        }
    }

    private void HandleLeaveRequest(LuaEvent luaEvent)
    {
        try
        {
            var player = (luaEvent.Player as TdmPlayer)!;
            var match = player.Match;

            if (match == null)
                return;

            this.logger.LogInformation("{player} left match \"{match}\" ({id})", player.Name, match.Name, match.Id);
            
            match.RemovePlayer(player);
            if (!match.Players.Any())
                this.matches.Remove(match);
        }
        catch (Exception e)
        {
            luaEvent.Player.SendErrorMessage("Unable to leave game");
            this.logger.LogError("{message}", e.Message);
        }
    }

    private void HandleCreateRequest(LuaEvent luaEvent)
    {
        try
        {
            var player = (luaEvent.Player as TdmPlayer)!;

            if (player.Match != null)
            {
                player.SendErrorMessage("You can not create a match because you are already in one.");
                return;
            }
            var luaValue = new CreateMatchLuaValue();
            luaValue.Parse(luaEvent.Parameters[0]);

            var match = new Match(this.matchId++, luaValue.Name, player);
            this.matches.Add(match);

            this.logger.LogInformation("{player} created match \"{match}\" ({id})", player.Name, match.Name, match.Id);
        }
        catch (Exception e)
        {
            luaEvent.Player.SendErrorMessage("Unable to create match");
            this.logger.LogError("{message}", e.Message);
        }
    }

    private void HandleStartMatchRequest(LuaEvent luaEvent)
    {
        try
        {
            var player = (luaEvent.Player as TdmPlayer)!;
            var match = player.Match;
            if (match != null && match.Host == player && match.CanStart)
            {
                match.Start();
                this.logger.LogInformation("{player} started the match \"{match}\" ({id})", player.Name, match.Name, match.Id);
            }
            else
                luaEvent.Player.SendErrorMessage("You can not start this match");
        }
        catch (Exception e)
        {
            luaEvent.Player.SendErrorMessage("Unable to start match");
            this.logger.LogError("{message}", e.Message);
        }
    }

    private void HandleSetMapRequest(LuaEvent luaEvent)
    {
        try
        {
            var luaValue = new SelectMapLuaValue();
            luaValue.Parse(luaEvent.Parameters[0]);

            var map = this.mapService.GetMap(luaValue.Name);

            var player = (luaEvent.Player as TdmPlayer)!;

            if (player.Match != null && player.Match.Host == player)
            {
                player.Match.SetMap(map);
                this.logger.LogInformation("{player} set the map to \"{map}\" for match \"{match}\" ({id})", player.Name, luaValue.Name, player.Match.Name, player.Match.Id);
            }
            else
                luaEvent.Player.SendErrorMessage("You can not set the map for this match");
        }
        catch (Exception e)
        {
            luaEvent.Player.SendErrorMessage("Unable to create game");
            this.logger.LogError("{message}", e.Message);
        }
    }

    private LuaValue GetMatchesLuaValue()
    {
        var table = new Dictionary<LuaValue, LuaValue>();
        int matchesIndex = 1;

        foreach (var match in this.matches)
        {
            table[matchesIndex++] = new LuaValue(new Dictionary<LuaValue, LuaValue>()
            {
                ["id"] = match.Id,
                ["name"] = match.Name,
                ["players"] = new LuaValue(match.Players
                    .Select(x => new LuaValue(x.Id))),
                ["mapName"] = match.Map?.Name ?? "N/A",
                ["state"] = match.State.ToString(),
                ["host"] = new LuaValue(match.Host.Id)
            });
        }

        return new LuaValue(table);
    }

    private LuaValue GetMapsLuaValue()
    {
        var table = new Dictionary<LuaValue, LuaValue>();
        int mapsIndex = 1;

        foreach (var map in this.mapService.AvailableMaps)
            table[mapsIndex++] = map;

        return new LuaValue(table);
    }
}
