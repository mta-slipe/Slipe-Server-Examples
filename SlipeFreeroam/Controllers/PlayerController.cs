using SlipeFreeroam.Elements;
using SlipeFreeroam.Services;
using SlipeServer.LuaControllers;
using SlipeServer.LuaControllers.Attributes;
using SlipeServer.Packets.Enums;
using SlipeServer.Server;
using SlipeServer.Server.Enums;

namespace SlipeFreeroam.Controllers;

[LuaController("", false)]
public class PlayerController : BaseLuaController<FreeroamPlayer>
{
    private readonly MtaServer server;
    private readonly ClothingService clothingService;

    public PlayerController(MtaServer server, ClothingService clothingService)
    {
        this.server = server;
        this.clothingService = clothingService;
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
    public void SetInterior(FreeroamPlayer player, int interior)
    {
        player.Interior = (byte)interior;
    }

    [LuaEvent("setElementAlpha")]
    public void SetAlpha(FreeroamPlayer player, int alpha)
    {
        player.Alpha = (byte)alpha;
    }

    [LuaEvent("setPedStat")]
    public void SetPedStat(FreeroamPlayer player, PedStat stat, float value)
    {
        player.SetStat(stat, value);
    }

    [LuaEvent("onClothesInit")]
    public void RequestClothes()
    {
        this.Context.Player.SendClothes(this.clothingService.GetClothingData(this.Context.Player));
    }

    [LuaEvent("addPedClothes")]
    public void AddClothes(FreeroamPlayer player, string texture, string model, ClothingType type)
    {
        this.clothingService.AddClothingToPlayer(player, model, texture);
    }

    [LuaEvent("removePedClothes")]
    public void RemoveClothes(FreeroamPlayer player, ClothingType type)
    {
        this.clothingService.RemoveClothingFromPlayer(player, type);
    }

    [LuaEvent("setPedAnimation")]
    public void SetAnimation(FreeroamPlayer player, string group, string animation, bool loops, bool updatesPosition)
    {

    }
}

