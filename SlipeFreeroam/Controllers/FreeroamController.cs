using SlipeFreeroam.Elements;
using SlipeServer.LuaControllers;
using SlipeServer.LuaControllers.Attributes;

namespace SlipeFreeroam.Controllers;

[LuaController("")]
public class FreeroamController : BaseLuaController<FreeroamPlayer>
{
    [LuaEvent("onLoadedAtClient")]
    public void HandleClientLoad()
    {
        this.Context.Player.ShowMap();
    }
}

