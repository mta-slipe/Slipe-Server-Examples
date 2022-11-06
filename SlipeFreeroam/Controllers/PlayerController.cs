using SlipeFreeroam.Elements;
using SlipeServer.LuaControllers;
using SlipeServer.LuaControllers.Attributes;
using SlipeServer.Packets.Definitions.Lua;
using SlipeServer.Server;
using SlipeServer.Server.Elements;
using SlipeServer.Server.Enums;

namespace SlipeFreeroam.Controllers;

[LuaController("")]
public class PlayerController : BaseLuaController<FreeroamPlayer>
{
    private readonly MtaServer server;

    public PlayerController(MtaServer server)
    {
        this.server = server;
    }

    [LuaEvent("onFreeroamSuicide")]
    public void RequestSuicide()
    {
        this.Context.Player.Kill();
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

    [LuaEvent("setMySkin")]
    public void SetSkin(int model)
    {
        var player = this.Context.Player;
        if (player.IsAlive)
        {
            player.Model = (ushort)model;
            return;
        }

        player.Spawn(player.Position, player.PedRotation, (ushort)model, player.Interior, player.Dimension);
    }

    [LuaEvent("spawnMe")]
    public void SpawnMe(int x, int y, int z)
    {
        var player = this.Context.Player;
        player.Spawn(new(x, y, z), player.PedRotation, player.Model, player.Interior, player.Dimension);
    }

    [LuaEvent("giveMeWeapon")]
    public void GiveWeapon(WeaponId weapon, int ammo)
    {
        this.Context.Player.AddWeapon(weapon, (ushort)ammo, true);
    }

    [LuaEvent("setElementInterior")]
    public void SetInterior(FreeroamPlayer element, int interior)
    {
        System.Console.WriteLine(interior);
        element.Interior = (byte)interior;
    }

    [LuaEvent("setElementAlpha")]
    public void SetAlpha(FreeroamPlayer element, int alpha)
    {
        element.Alpha = (byte)alpha;
    }
}

