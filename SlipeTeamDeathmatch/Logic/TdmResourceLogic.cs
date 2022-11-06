using SlipeServer.Server;
using SlipeServer.Server.Elements;
using SlipeServer.Server.Elements.Events;
using SlipeServer.Server.Resources;
using SlipeServer.Server.Resources.Providers;
using SlipeServer.Server.Services;
using SlipeTeamDeathmatch.Elements;
using SlipeTeamDeathmatch.Services;

namespace SlipeTeamDeathmatch.Logic;

public class TdmResourceLogic
{
    private readonly MatchService matchService;
    private Resource resource;

    public TdmResourceLogic(MtaServer<TdmPlayer> mtaServer,
        IResourceProvider resourceProvider,
        CommandService commandService,
        MatchService matchService)
    {
        this.matchService = matchService;

        this.resource = resourceProvider.GetResource("SlipeTdm");
        commandService.AddCommand("restart").Triggered += (s, e) =>
        {
            this.resource.Stop();
            this.resource = resourceProvider.GetResource("SlipeTdm");
            this.resource.Start();
        };

        mtaServer.PlayerJoined += HandlePlayerJoin;
    }

    private async void HandlePlayerJoin(TdmPlayer player)
    {
        await this.resource.StartForAsync(player);

        if (player.Match == null)
            player.SendMatches(this.matchService.Matches);
        else
            player.SendMatch(player.Match);

        if (!player.Account.IsGuest)
            player.SendLoggedIn();
    }
}
