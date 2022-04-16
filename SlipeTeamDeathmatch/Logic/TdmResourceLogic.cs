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

    public TdmResourceLogic(MtaServer mtaServer,
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
                player.SendMatches(this.matchService.Matches);
            else
                player.SendMatch(player.Match);

            if (!player.Account.IsGuest)
                player.SendLoggedIn();
        }
    }
}
