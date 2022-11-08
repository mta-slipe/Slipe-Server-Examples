using SlipeFreeroam.Services;
using SlipeServer.Server;
using SlipeServer.Server.Elements;
using SlipeServer.Server.Extensions;
using SlipeServer.Server.Services;

namespace SlipeFreeroam.Elements;

public class FreeroamPlayer : Player
{
    private readonly MtaServer<FreeroamPlayer> server;
    private readonly LuaEventService luaEventService;
    private readonly RootElement root;

    public List<Vehicle> Vehicles { get; }
    public Blip Blip { get; }

    public DateTime LastMessageTime { get; set; }
    public string LastMessage { get; set; } = "";

    public FreeroamPlayer(MtaServer<FreeroamPlayer> server, LuaEventService luaEventService, RootElement root)
    {
        this.server = server;
        this.luaEventService = luaEventService;
        this.root = root;

        this.Vehicles = new();
        this.Blip = new Blip(this.position, BlipIcon.Marker).AssociateWith(server);
        this.Blip.AttachTo(this);

        this.Name = Name.StripColorCode();

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

    public void SendClothes(ClothingData clothingData)
    {
        this.luaEventService.TriggerEventFor(this, "onClientClothesInit", this.root, clothingData);
    }
}