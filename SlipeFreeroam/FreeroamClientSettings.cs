using SlipeServer.Packets.Definitions.Lua;
using SlipeServer.Server.Mappers;

namespace SlipeFreeroam;

public class FreeroamClientSettings : ILuaMappable
{
    public bool IsCommandSpamProtectionEnabled { get; set; } = true;
    public int CommandSpamProtectionThreshold { get; set; } = 7;
    public int CommandSpamProtectionLowPriorityThreshold { get; set; } = 10;
    public TimeSpan CommandSpamProtectionBanDuration { get; set; } = TimeSpan.FromSeconds(10);
    public List<string> CommandSpamProtectionExceptions { get; set; } = new()
    {
        "sp", "setpos", "repair", "rp", "anim", "speed", "jp", "addupgrade", "au"
    };
    public bool RemoveColorCodesFromNames { get; set; } = false;
    public bool ShowSpawnMapOnDeath { get; set; } = false;
    public bool AreKnifeRestrictionsEnabled { get; set; } = true;
    public bool IsKillEnabled { get; set; } = true;
    public bool IsWarpEnabled { get; set; } = true;
    public bool HideColorText { get; set; } = true;
    public bool CanGameSpeedBeModified { get; set; } = true;
    public float MinGameSpeed { get; set; } = 0;
    public float MaxGameSpeed { get; set; } = 10;
    public bool IsAntiRamEnabled { get; set; } = true;
    public bool IsDisableWarpEnabled { get; set; } = true;
    public bool IsKnifeEnabled { get; set; } = true;
    public List<int> DisallowedVehicleWarps { get; set; } = new()
    {
        425, 520, 476, 447, 464, 432
    };

    public LuaValue ToLuaValue()
    {
        return new Dictionary<LuaValue, LuaValue>()
        {
            ["command_spam_protection"] = this.IsCommandSpamProtectionEnabled,
            ["tries_required_to_trigger"] = this.CommandSpamProtectionThreshold,
            ["tries_required_to_trigger_low_priority"] = this.CommandSpamProtectionLowPriorityThreshold,
            ["command_spam_ban_duration"] = this.CommandSpamProtectionBanDuration.TotalMilliseconds,
            ["command_exception_commands"] = this.CommandSpamProtectionExceptions.Select(x => (LuaValue)x).ToArray(),
            ["removeHex"] = this.RemoveColorCodesFromNames,
            ["spawnmapondeath"] = this.ShowSpawnMapOnDeath,
            ["weapons/kniferestrictions"] = this.AreKnifeRestrictionsEnabled,
            ["kill"] = this.IsKillEnabled,
            ["warp"] = this.IsWarpEnabled,
            ["hidecolortext"] = this.HideColorText,
            ["gamespeed/enabled"] = this.CanGameSpeedBeModified,
            ["gamespeed/min"] = this.MinGameSpeed,
            ["gamespeed/max"] = this.MaxGameSpeed,
            ["gui/antiram"] = this.IsAntiRamEnabled,
            ["gui/disablewarp"] = this.IsDisableWarpEnabled,
            ["gui/disableknife"] = this.IsKnifeEnabled,
            ["vehicles/disallowed_warp"] = this.DisallowedVehicleWarps.Select(x => (LuaValue)x).ToArray(),
        };
    }

    public IEnumerable<(string, LuaValue)> GetSettings()
    {
        yield return ("command_spam_protection", this.IsCommandSpamProtectionEnabled);
        yield return ("tries_required_to_trigger", this.CommandSpamProtectionThreshold);
        yield return ("tries_required_to_trigger_low_priority", this.CommandSpamProtectionLowPriorityThreshold);
        yield return ("command_spam_ban_duration", this.CommandSpamProtectionBanDuration.TotalMilliseconds);
        yield return ("command_exception_commands", this.CommandSpamProtectionExceptions.Select(x => (LuaValue)x).ToArray());
        yield return ("removeHex", this.RemoveColorCodesFromNames);
        yield return ("spawnmapondeath", this.ShowSpawnMapOnDeath);
        yield return ("weapons/kniferestrictions", this.AreKnifeRestrictionsEnabled);
        yield return ("kill", this.IsKillEnabled);
        yield return ("warp", this.IsWarpEnabled);
        yield return ("hidecolortext", this.HideColorText);
        yield return ("gamespeed/enabled", this.CanGameSpeedBeModified);
        yield return ("gamespeed/min", this.MinGameSpeed);
        yield return ("gamespeed/max", this.MaxGameSpeed);
        yield return ("gui/antiram", this.IsAntiRamEnabled);
        yield return ("gui/disablewarp", this.IsDisableWarpEnabled);
        yield return ("gui/disableknife", this.IsKnifeEnabled);
        yield return ("vehicles/disallowed_warp", this.DisallowedVehicleWarps.Select(x => (LuaValue)x).ToArray());
    }
}
