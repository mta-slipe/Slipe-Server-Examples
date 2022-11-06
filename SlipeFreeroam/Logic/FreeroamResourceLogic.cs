using SlipeFreeroam.Elements;
using SlipeServer.Server;
using SlipeServer.Server.Resources;
using SlipeServer.Server.Resources.Providers;
using SlipeServer.Server.Services;

namespace SlipeFreeroam.Logic;

public class FreeroamResourceLogic
{
    private readonly Resource freeroamResource;
    private readonly FreeroamClientSettings clientSettings;
    private readonly DebugLog debugLog;

    public FreeroamResourceLogic(
        MtaServer<FreeroamPlayer> server,
        IResourceProvider resourceProvider,
        FreeroamClientSettings clientSettings,
        DebugLog debugLog)
    {
        this.freeroamResource = resourceProvider.GetResource("freeroam");

        server.PlayerJoined += HandlePlayerJoin;
        this.clientSettings = clientSettings;
        this.debugLog = debugLog;
    }

    private async void HandlePlayerJoin(FreeroamPlayer player)
    {
        this.debugLog.SetVisibleTo(player, true);

        await this.freeroamResource.StartForAsync(player);
        SendSettings(player);
    }

    private void SendSettings(FreeroamPlayer player)
    {

    }
}
