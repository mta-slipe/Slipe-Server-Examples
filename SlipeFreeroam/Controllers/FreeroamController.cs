using SlipeFreeroam.Elements;
using SlipeServer.LuaControllers;
using SlipeServer.LuaControllers.Attributes;
using SlipeServer.Server.ElementCollections;

namespace SlipeFreeroam.Controllers;

[LuaController("")]
public class FreeroamController : BaseLuaController<FreeroamPlayer>
{
    private readonly IElementCollection elementCollection;

    public FreeroamController(IElementCollection elementCollection)
    {
        this.elementCollection = elementCollection;
    }

    [LuaEvent("onLoadedAtClient")]
    public void HandleClientLoad()
    {
        this.Context.Player.ShowMap();

        foreach (var player in this.elementCollection.GetByType<FreeroamPlayer>())
            foreach (var (setting, value) in player.PersonalSettings)
                this.Context.Player.SendSetting(player, setting, value);
    }
}

