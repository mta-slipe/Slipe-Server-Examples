using SlipeServer.Server;
using SlipeServer.Server.Elements;
using SlipeServer.Server.Extensions;
using SlipeServer.Server.Services;

namespace SlipeFreeroam.Elements;

public class FreeroamPlayer : Player
{
    private readonly MtaServer<FreeroamPlayer> server;

    public List<Vehicle> Vehicles { get; }
    public Blip Blip { get; }

    public DateTime LastMessageTime { get; set; }
    public string LastMessage { get; set; } = "";

    public FreeroamPlayer(MtaServer<FreeroamPlayer> server)
    {
        this.server = server;

        Vehicles = new();
        Blip = new Blip(this.position, BlipIcon.Marker).AssociateWith(server);
        Blip.AttachTo(this);

        Name = Name.StripColorCode();

        Destroyed += HandleDestroy;
    }

    private void HandleDestroy(Element obj)
    {
        Blip.Destroy();

        foreach (var vehicle in Vehicles)
            vehicle.Destroy();
    }

    public void ShowMap()
    {
        TriggerLuaEvent("onClientCall", this, "showWelcomeMap");
    }
}