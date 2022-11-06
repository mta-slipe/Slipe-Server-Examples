using System.Drawing;
using Microsoft.Extensions.Logging;
using SlipeFreeroam.Elements;
using SlipeServer.Packets.Enums;
using SlipeServer.Server;
using SlipeServer.Server.Services;

namespace SlipeFreeroam.Behaviour;
public class ChatBehaviour
{
    public ChatBehaviour(MtaServer<FreeroamPlayer> server, ChatBox chatBox, ILogger? logger)
    {
        server.PlayerJoined += (player) =>
        {
            player.CommandEntered += (sender, arguments) =>
            {
                if (arguments.Command == "say")
                {
                    string message = string.Join(' ', arguments.Arguments);

                    if (message == player.LastMessage)
                    {
                        chatBox.OutputTo(player, "Stop repeating yourself!", Color.Red, true, ChatEchoType.Player, player);
                        return;
                    }
                    if ((DateTime.UtcNow - player.LastMessageTime).TotalMilliseconds < 1000)
                    {
                        chatBox.OutputTo(player, "Stop spamming main chat!", Color.Red, true, ChatEchoType.Player, player);
                        return;
                    }

                    player.LastMessage = message;
                    player.LastMessageTime = DateTime.UtcNow;

                    var color = (player.NametagColor ?? Color.White);
                    var colorCode = $"#{color.R:X2}{color.G:X2}{color.B:X2}";
                    string prefix = $"{colorCode}{player.Name}:#ffffff";
                    string fullMessage = $"{prefix} {message}";

                    chatBox.Output(fullMessage, Color.White, true, ChatEchoType.Player, player);
                    logger?.LogInformation("{message}", $"{player.Name}: message");
                }
            };
        };
    }
}
