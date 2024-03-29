﻿using SlipeFreeroam.Services;
using SlipeServer.Packets.Definitions.Lua;
using SlipeServer.Server;
using SlipeServer.Server.Elements;
using SlipeServer.Server.Extensions;
using SlipeServer.Server.Resources;
using SlipeServer.Server.Resources.Providers;
using SlipeServer.Server.Services;

namespace SlipeFreeroam.Elements;

public class FreeroamPlayer : Player
{
    private readonly MtaServer<FreeroamPlayer> server;
    private readonly LuaEventService luaEventService;
    private readonly Resource resource;

    public List<Vehicle> Vehicles { get; }
    public Blip Blip { get; }

    public DateTime LastMessageTime { get; set; }
    public string LastMessage { get; set; } = "";
    public Dictionary<string, bool> PersonalSettings { get; set; }

    public FreeroamPlayer(
        MtaServer<FreeroamPlayer> server,
        LuaEventService luaEventService,
        IResourceProvider resourceProvider)
    {
        this.server = server;
        this.luaEventService = luaEventService;
        this.resource = resourceProvider.GetResource("freeroam");

        this.Vehicles = new();
        this.PersonalSettings = new();
        this.Blip = this.CreateBlipFor(BlipIcon.Marker).AssociateWith(server);

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
        TriggerLuaEvent("onClientCall", this.resource.DynamicRoot, "showWelcomeMap");
    }

    public void SendClothes(ClothingData clothingData)
    {
        this.luaEventService.TriggerEventFor(this, "onClientClothesInit", this.resource.DynamicRoot, clothingData);
    }

    public void SendSetting(FreeroamPlayer player, string setting, bool value)
    {
        this.luaEventService.TriggerEventFor(this, "onClientFreeroamLocalSettingChange", player, setting, value);
    }

    public void SendGlobalSettings(FreeroamClientSettings settings)
    {
        this.luaEventService.TriggerEventFor(this, "onClientCall", this.resource.Root, "freeroamSettings", settings);
        this.luaEventService.TriggerEventFor(this, "onClientCall", this.resource.DynamicRoot, "freeroamSettings", settings);
    }
}