using SlipeFreeroam.Elements;
using SlipeServer.LuaControllers;
using SlipeServer.LuaControllers.Attributes;
using SlipeServer.Server;

namespace SlipeFreeroam.Controllers;

[LuaController("")]
public class FreeroamController : BaseLuaController<FreeroamPlayer>
{
    private readonly MtaServer server;

    public FreeroamController(MtaServer server)
    {
        this.server = server;
    }

    [LuaEvent("onFreeroamSuicide")]
    public void RequestSuicide()
    {
        this.Context.Player.Kill();
    }

    [LuaEvent("onLoadedAtClient")]
    public void HandleClientLoad()
    {
        this.Context.Player.ShowMap();
    }

    [LuaEvent("onPlayerGravInit")]
    public void HandleGravInit()
    {
        //Context.Player.TriggerLuaEvent("onClientPlayerGravInit", Context.Player, Context.Player.Gravity);
    }

    [LuaEvent("givePedJetPack")]
    public void GiveJetpack() => this.Context.Player.HasJetpack = true;

    [LuaEvent("removePedJetPack")]
    public void RemoveJetpack() => this.Context.Player.HasJetpack = false;

    [LuaEvent("onFreeroamLocalSettingChange")]
    public void HandleSettingChange(string setting, bool value)
    {

    }
}

