using SlipeFreeroam.Elements;
using SlipeServer.Packets.Enums.VehicleUpgrades;
using SlipeServer.Server.Constants;
using SlipeServer.Server.Elements;

namespace SlipeFreeroam.Services;

public class VehicleUpgradeService
{
    private readonly Dictionary<ushort, Type> upgradeTypePerId;

    public VehicleUpgradeService()
    {
        this.upgradeTypePerId = new();
        InitUpgradeTypes();
    }

    private void InitUpgradeTypes()
    {
        foreach (var upgrade in VehicleUpgradeConstants.HoodUpgradeIds.SelectMany(x => x.Value))
            this.upgradeTypePerId[upgrade] = typeof(VehicleUpgradeHood);

        foreach (var upgrade in VehicleUpgradeConstants.VentUpgradeIds.SelectMany(x => x.Value))
            this.upgradeTypePerId[upgrade] = typeof(VehicleUpgradeVent);

        foreach (var upgrade in VehicleUpgradeConstants.SpoilerUpgradeIds.SelectMany(x => x.Value))
            this.upgradeTypePerId[upgrade] = typeof(VehicleUpgradeSpoiler);

        foreach (var upgrade in VehicleUpgradeConstants.SideskirtUpgradeIds.SelectMany(x => x.Value))
            this.upgradeTypePerId[upgrade] = typeof(VehicleUpgradeSideskirt);

        foreach (var upgrade in VehicleUpgradeConstants.FrontBullbarIds.SelectMany(x => x.Value))
            this.upgradeTypePerId[upgrade] = typeof(VehicleUpgradeFrontBullbar);

        foreach (var upgrade in VehicleUpgradeConstants.RearBullbarIds.SelectMany(x => x.Value))
            this.upgradeTypePerId[upgrade] = typeof(VehicleUpgradeRearBullbar);

        foreach (var upgrade in VehicleUpgradeConstants.LampIds.SelectMany(x => x.Value))
            this.upgradeTypePerId[upgrade] = typeof(VehicleUpgradeLamp);

        foreach (var upgrade in VehicleUpgradeConstants.RoofIds.SelectMany(x => x.Value))
            this.upgradeTypePerId[upgrade] = typeof(VehicleUpgradeRoof);

        foreach (var upgrade in VehicleUpgradeConstants.WheelUpgradeIds.SelectMany(x => x.Value))
            this.upgradeTypePerId[upgrade] = typeof(VehicleUpgradeWheel);

        foreach (var upgrade in VehicleUpgradeConstants.ExhaustUpgradeIds.SelectMany(x => x.Value))
            this.upgradeTypePerId[upgrade] = typeof(VehicleUpgradeExhaust);

        foreach (var upgrade in VehicleUpgradeConstants.FrontBumperUpgradeIds.SelectMany(x => x.Value))
            this.upgradeTypePerId[upgrade] = typeof(VehicleUpgradeFrontBumper);

        foreach (var upgrade in VehicleUpgradeConstants.RearBumperUpgradeIds.SelectMany(x => x.Value))
            this.upgradeTypePerId[upgrade] = typeof(VehicleUpgradeRearBumper);

        foreach (var upgrade in VehicleUpgradeConstants.MiscUpgradeIds.SelectMany(x => x.Value))
            this.upgradeTypePerId[upgrade] = typeof(VehicleUpgradeMisc);
    }

    public void ApplyUpgrade(FreeroamVehicle vehicle, int upgrade)
    {
        if (!this.upgradeTypePerId.TryGetValue((ushort)upgrade, out var type))
            return;

        ApplyUpgrade(vehicle, type, (ushort)upgrade);

    }

    public void RemoveUpgrade(FreeroamVehicle vehicle, int upgrade)
    {
        if (!this.upgradeTypePerId.TryGetValue((ushort)upgrade, out var type))
            return;

        ApplyUpgrade(vehicle, type, 0);
    }

    private void ApplyUpgrade(FreeroamVehicle vehicle, Type type, ushort upgrade)
    {
        if (type == typeof(VehicleUpgradeHood))
            vehicle.Upgrades.Hood = GetUpgradeEnum(VehicleUpgradeConstants.HoodUpgradeIds, upgrade);

        else if (type == typeof(VehicleUpgradeVent))
            vehicle.Upgrades.Vent = GetUpgradeEnum(VehicleUpgradeConstants.VentUpgradeIds, upgrade);

        else if (type == typeof(VehicleUpgradeSpoiler))
            vehicle.Upgrades.Spoiler = GetUpgradeEnum(VehicleUpgradeConstants.SpoilerUpgradeIds, upgrade);

        else if (type == typeof(VehicleUpgradeSideskirt))
            vehicle.Upgrades.Sideskirt = GetUpgradeEnum(VehicleUpgradeConstants.SideskirtUpgradeIds, upgrade);

        else if (type == typeof(VehicleUpgradeFrontBullbar))
            vehicle.Upgrades.FrontBullbar = GetUpgradeEnum(VehicleUpgradeConstants.FrontBullbarIds, upgrade);

        else if (type == typeof(VehicleUpgradeRearBullbar))
            vehicle.Upgrades.RearBullbar = GetUpgradeEnum(VehicleUpgradeConstants.RearBullbarIds, upgrade);

        else if (type == typeof(VehicleUpgradeLamp))
            vehicle.Upgrades.Lamps = GetUpgradeEnum(VehicleUpgradeConstants.LampIds, upgrade);

        else if (type == typeof(VehicleUpgradeRoof))
            vehicle.Upgrades.Roof = GetUpgradeEnum(VehicleUpgradeConstants.RoofIds, upgrade);

        else if (type == typeof(VehicleUpgradeNitro))
            vehicle.Upgrades.Nitro = GetUpgradeEnum(VehicleUpgradeConstants.NitroIds, upgrade);

        else if (type == typeof(VehicleUpgradeWheel))
            vehicle.Upgrades.Wheels = GetUpgradeEnum(VehicleUpgradeConstants.WheelUpgradeIds, upgrade);

        else if (type == typeof(VehicleUpgradeExhaust))
            vehicle.Upgrades.Exhaust = GetUpgradeEnum(VehicleUpgradeConstants.ExhaustUpgradeIds, upgrade);

        else if (type == typeof(VehicleUpgradeFrontBumper))
            vehicle.Upgrades.FrontBumper = GetUpgradeEnum(VehicleUpgradeConstants.FrontBumperUpgradeIds, upgrade);

        else if (type == typeof(VehicleUpgradeRearBumper))
            vehicle.Upgrades.RearBumper = GetUpgradeEnum(VehicleUpgradeConstants.RearBumperUpgradeIds, upgrade);

        else if (type == typeof(VehicleUpgradeMisc))
            vehicle.Upgrades.Misc = GetUpgradeEnum(VehicleUpgradeConstants.MiscUpgradeIds, upgrade);
    }

    private T GetUpgradeEnum<T>(Dictionary<T, ushort[]> dictionary, ushort target) where T : notnull, System.Enum
    {
        return target == 0 ? (T)Enum.ToObject(typeof(T), 0) : dictionary.Where(x => x.Value.Contains(target)).Single().Key;
    }
}
