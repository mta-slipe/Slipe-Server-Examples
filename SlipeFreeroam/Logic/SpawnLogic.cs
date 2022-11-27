using SlipeFreeroam.Elements;
using SlipeServer.Packets.Lua.Camera;
using SlipeServer.Server;
using SlipeServer.Server.Elements;
using SlipeServer.Server.Elements.Enums;
using SlipeServer.Server.Elements.Events;
using SlipeServer.Server.Services;
using System.Drawing;
using System.Numerics;

namespace SlipeFreeroam.Logic;

public class SpawnLogic
{
    private readonly Random random = new();
    private readonly ChatBox chatBox;

    public SpawnLogic(MtaServer<FreeroamPlayer> server, ChatBox chatBox)
    {
        this.chatBox = chatBox;

        server.PlayerJoined += HandlePlayerJoin;
    }

    private void HandlePlayerJoin(FreeroamPlayer player)
    {
        var color = GetRandomColor();
        player.NametagColor = color;
        player.Blip.Color = color;

        player.Spawn(new Vector3(0, 0, 3), 0, (ushort)PedModel.Cj, 0, 0);
        player.Camera.Target = player;
        player.Camera.Fade(CameraFade.In);

        this.chatBox.OutputTo(player, "Welcome to Freeroam", Color.Green);
        this.chatBox.OutputTo(player, "Press F1 to show/hide control", Color.Green);

        player.Wasted += HandlePlayerWasted;
    }

    private Color GetRandomColor()
    {
        var buffer = new byte[3];
        this.random.NextBytes(buffer);
        return Color.FromArgb(255, buffer[0], buffer[1], buffer[2]);
    }

    private void HandlePlayerWasted(Ped sender, PedWastedEventArgs e)
    {
        (sender as Player)?.Spawn(new Vector3(0, 0, 3), 0, (ushort)PedModel.Sweet, 0, 0);
    }
}
