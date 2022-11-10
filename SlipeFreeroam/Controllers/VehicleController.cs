using System.Drawing;
using System.Numerics;
using SlipeFreeroam.Elements;
using SlipeFreeroam.Services;
using SlipeServer.LuaControllers;
using SlipeServer.LuaControllers.Attributes;
using SlipeServer.Packets.Lua.Camera;
using SlipeServer.Server;
using SlipeServer.Server.Elements;

namespace SlipeFreeroam.Controllers;

[LuaController("")]
public class VehicleController : BaseLuaController<FreeroamPlayer>
{
    private readonly MtaServer server;
    private readonly VehicleUpgradeService vehicleUpgradeService;

    public VehicleController(MtaServer server, VehicleUpgradeService vehicleUpgradeService)
    {
        this.server = server;
        this.vehicleUpgradeService = vehicleUpgradeService;
    }

    [LuaEvent("giveMeVehicles")]
    public void CreateVehicleForPlayer(int model)
    {
        var vehicle = new FreeroamVehicle((ushort)model, this.Context.Player.Position + new Vector3(0, 0, 2))
        {
            Rotation = this.Context.Player.Rotation,
            Interior = this.Context.Player.Interior,
            Dimension = this.Context.Player.Dimension,
        };

        vehicle.AssociateWith(this.server);
        vehicle.Driver = this.Context.Player;
        this.Context.Player.Vehicles.Add(vehicle);
    }

    [LuaEvent("setVehiclePaintjob")]
    public void SetPaintjob(FreeroamVehicle vehicle, int paintjob)
    {
        vehicle.PaintJob = (byte)(paintjob - 1);
    }

    [LuaEvent("setVehicleColor")]
    public void SetVehicleColor(FreeroamVehicle vehicle, int r1, int g1, int b1, int r2, int g2, int b2, int r3, int g3, int b3, int r4, int g4, int b4)
    {
        vehicle.Colors.Primary = Color.FromArgb(255, r1, g1, b1);
        vehicle.Colors.Secondary = Color.FromArgb(255, r2, g2, b2);
        vehicle.Colors.Color3 = Color.FromArgb(255, r3, g3, b3);
        vehicle.Colors.Color4 = Color.FromArgb(255, r4, g4, b4);
    }

    [LuaEvent("setVehicleHeadlightColor")]
    public void SetHeadlightColor(FreeroamVehicle vehicle, int r, int g, int b)
    {
        vehicle.HeadlightColor = Color.FromArgb(255, r, g, b);
    }

    [LuaEvent("fixVehicle")]
    public void FixVehicle(FreeroamVehicle vehicle)
    {
        vehicle.Health = 1000;
        vehicle.ResetDoorsWheelsPanelsLights();
    }

    [LuaEvent("fadeVehiclePassengersCamera")]
    public void FadePassengerCamera(bool fade)
    {
        var vehicle = this.Context.Player.Vehicle;

        if (vehicle == null)
            return;

        foreach (var occupant in vehicle.Occupants)
            if (occupant.Value is Player player)
                player.Camera.Fade(fade ? CameraFade.In : CameraFade.Out);
    }

    [LuaEvent("warpMeIntoVehicle")]
    public void WarpIntoVehicle(Vehicle vehicle)
    {
        var seat = vehicle.GetFreePassengerSeat();
        if (seat != null)
        {
            var player = this.Context.Player;
            if (!player.IsAlive)
                player.Spawn(vehicle.Position + Vector3.UnitZ, player.PedRotation, player.Model, player.Interior, player.Dimension);

            player.WarpIntoVehicle(vehicle, seat.Value);
        }
    }

    [LuaEvent("setVehicleOverrideLights")]
    public void SetOverrideLights(Vehicle vehicle, int lights)
    {
        vehicle.OverrideLights = (VehicleOverrideLights)lights;
    }

    [LuaEvent("addVehicleUpgrade")]
    public void AddUpgrade(FreeroamVehicle vehicle, int upgrade)
    {
        this.vehicleUpgradeService.ApplyUpgrade(vehicle, upgrade);
    }

    [LuaEvent("removeVehicleUpgrade")]
    public void RemoveUpgrade(FreeroamVehicle vehicle, int upgrade)
    {
        this.vehicleUpgradeService.RemoveUpgrade(vehicle, upgrade);
    }
}

