using Microsoft.Extensions.Logging;
using SlipeTeamDeathmatch.Elements;
using SlipeTeamDeathmatch.Models;

namespace SlipeTeamDeathmatch.Services;
public class MatchService
{
    private int matchId;
    private readonly ILogger logger;
    private readonly MapService mapService;

    private readonly List<Match> matches;
    public IReadOnlyCollection<Match> Matches => matches.AsReadOnly();

    public MatchService(
        ILogger logger,
        MapService mapProvider)
    {
        this.logger = logger;
        this.mapService = mapProvider;

        this.matches = new();
    }

    public Match Get(int id)
    {
        return this.matches.Single(x => x.Id == id);
    }

    public void CreateMatch(TdmPlayer player, string name)
    {
        if (player.Match != null)
        {
            player.SendErrorMessage("You can not create a match because you are already in one.");
            return;
        }

        if (string.IsNullOrEmpty(name.Trim()))
        {
            player.SendErrorMessage("You must enter a name for your match.");
            return;
        }

        var match = new Match(this.matchId++, name, player);
        this.matches.Add(match);
        match.Abandoned += (sender, args) =>
        {
            this.matches.Remove(match);
            this.logger.LogInformation("Match \"{match}\" was abandoned", match.Name);
        };

        player.SendMatch(match);

        this.logger.LogInformation("{player} created match \"{match}\" ({id})", player.Name, match.Name, match.Id);
    }

    public void AddPlayerToMatch(TdmPlayer player, Match match)
    {
        if (player.Match != null)
        {
            player.SendErrorMessage("You can not join this match as you are already in a match.");
            return;
        }

        match.AddPlayer(player);
        this.logger.LogInformation("{player} joined match \"{match}\" ({id})", player.Name, match.Name, match.Id);
    }

    public void RemovePlayerFromMatch(TdmPlayer player)
    {
        var match = player.Match;
        if (match == null)
            return;

        this.logger.LogInformation("{player} left match \"{match}\" ({id})", player.Name, match.Name, match.Id);

        match.RemovePlayer(player);
        player.SendMatches(this.matches);
    }

    public void StartMatch(TdmPlayer player, Match match)
    {
        if (match != null && match.Host == player && match.CanStart)
        {
            match.Start();
            this.logger.LogInformation("{player} started the match \"{match}\" ({id})", player.Name, match.Name, match.Id);
        }
        else
            player.SendErrorMessage("You can not start this match");
    }

    public void SetMatchMap(TdmPlayer player, Match match, string mapName)
    {
        var map = this.mapService.GetMap(mapName);

        if (player.Match == match && player.Match.Host == player)
        {
            player.Match.SetMap(map);
            this.logger.LogInformation("{player} set the map to \"{map}\" for match \"{match}\" ({id})", player.Name, mapName, player.Match.Name, player.Match.Id);

            foreach (var matchPlayer in player.Match.Players)
                matchPlayer.SendMatch(player.Match);
        }
        else
            player.SendErrorMessage("You can not set the map for this match");
    }
}
