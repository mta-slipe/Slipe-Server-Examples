using System.Drawing;
using Microsoft.Extensions.Logging;
using SlipeFreeroam.Elements;
using SlipeServer.Packets.Enums;
using SlipeServer.Server.Events;
using SlipeServer.Server.Services;

namespace SlipeFreeroam.Logic;

public class ChatLogic
{
    private readonly ChatBox chatBox;
    private readonly ILogger logger;

    public ChatLogic(CommandService commandService, ChatBox chatBox, ILogger logger)
    {
        this.chatBox = chatBox;
        this.logger = logger;

        commandService.AddCommand("say").Triggered += HandleChatCommand;
    }

    private void HandleChatCommand(object? sender, CommandTriggeredEventArgs arguments)
    {
        if (arguments.Player is not FreeroamPlayer player)
            return;

        string message = string.Join(' ', arguments.Arguments);

        if (message == player.LastMessage)
        {
            this.chatBox.OutputTo(player, "Stop repeating yourself!", Color.Red, true, ChatEchoType.Player, player);
            return;
        }
        if ((DateTime.UtcNow - player.LastMessageTime).TotalMilliseconds < 1000)
        {
            this.chatBox.OutputTo(player, "Stop spamming main chat!", Color.Red, true, ChatEchoType.Player, player);
            return;
        }

        player.LastMessage = message;
        player.LastMessageTime = DateTime.UtcNow;

        var color = (player.NametagColor ?? Color.White);
        var colorCode = $"#{color.R:X2}{color.G:X2}{color.B:X2}";
        string prefix = $"{colorCode}{player.Name}:#ffffff";
        string fullMessage = $"{prefix} {message}";

        this.chatBox.Output(fullMessage, Color.White, true, ChatEchoType.Player, player);
        this.logger?.LogInformation("{message}", $"{player.Name}: {message}");
    }
}
