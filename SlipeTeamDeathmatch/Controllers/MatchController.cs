using SlipeServer.LuaControllers;
using SlipeServer.LuaControllers.Attributes;
using SlipeTeamDeathmatch.Elements;
using SlipeTeamDeathmatch.Services;

namespace SlipeTeamDeathmatch.Controllers;

[LuaController("Slipe.TeamDeathMatch.")]
public class MatchController : BaseLuaController<TdmPlayer>
{
    private readonly MapService mapService;
    private readonly MatchService matchService;

    public MatchController(
        MapService mapProvider,
        MatchService matchService)
    {
        this.mapService = mapProvider;
        this.matchService = matchService;
    }

    public void RequestMatches()
    {
        this.Context.Player.SendMatches(this.matchService.Matches);
    }

    public void RequestMaps()
    {
        this.Context.Player.SendMaps(this.mapService.AvailableMaps);
    }

    public void CreateMatch(string name)
    {
        this.matchService.CreateMatch(this.Context.Player, name);
    }

    public void JoinMatch(int id)
    {
        var match = this.matchService.Get(id);
        this.matchService.AddPlayerToMatch(this.Context.Player, match);
    }

    public void LeaveMatch()
    {
        this.matchService.RemovePlayerFromMatch(this.Context.Player);
    }

    public void StartMatch()
    {
        if (this.Context.Player.Match != null)
            this.matchService.StartMatch(this.Context.Player, this.Context.Player.Match);
    }

    public void SetMap(string name)
    {
        if (this.Context.Player.Match != null)
            this.matchService.SetMatchMap(this.Context.Player, this.Context.Player.Match, name);
    }
}
