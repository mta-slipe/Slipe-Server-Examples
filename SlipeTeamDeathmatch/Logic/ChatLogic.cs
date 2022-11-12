using System.Drawing;
using Microsoft.Extensions.Logging;
using SlipeServer.Packets.Enums;
using SlipeServer.Server.Events;
using SlipeServer.Server.Services;
using SlipeTeamDeathmatch.Elements;

namespace SlipeTeamDeathmatch.Logic;

public class ChatLogic
{
    private readonly ChatBox chatBox;
    private readonly ILogger logger;

    public ChatLogic(CommandService commandService, ChatBox chatBox, ILogger logger)
    {
        this.chatBox = chatBox;
        this.logger = logger;

        commandService.AddCommand("say").Triggered += HandleChatCommand;
        commandService.AddCommand("teamsay").Triggered += HandleTeamChatCommand;
        commandService.AddCommand("matchsay").Triggered += HandleMatchChatCommand;
    }

    private void HandleChatCommand(object? sender, CommandTriggeredEventArgs arguments)
    {
        if (arguments.Player is not TdmPlayer player)
            return;

        string message = string.Join(' ', arguments.Arguments);

        var color = player.Team?.Color ?? Color.White;
        var colorCode = $"#{color.R:X2}{color.G:X2}{color.B:X2}";
        string prefix = $"#aaaaaa(GLOBAL) {colorCode}{player.Name}:#ffffff";
        string fullMessage = $"{prefix} {message}";

        this.chatBox.Output(fullMessage, Color.White, true, ChatEchoType.Player, player);
        this.logger?.LogInformation("(GLOBAL) {message}", $"{player.Name}: {message}");
    }

    private void HandleTeamChatCommand(object? sender, CommandTriggeredEventArgs arguments)
    {
        if (arguments.Player is not TdmPlayer player)
            return;

        if (player.Team == null)
            return;

        string message = string.Join(' ', arguments.Arguments);

        var color = player.Team.Color;
        var colorCode = $"#{color.R:X2}{color.G:X2}{color.B:X2}";
        string prefix = $"{colorCode}(TEAM)#aaaaaa {player.Name}:#ffffff";
        string fullMessage = $"{prefix} {message}";

        foreach (var teamPlayer in player.Team.Players)
            this.chatBox.OutputTo(teamPlayer, fullMessage, Color.White, true, ChatEchoType.Team, player);

        this.logger?.LogInformation("({match}-{team}){message}", player.Match!.Name, player.Team.TeamName, $"{player.Name}: {message}");
    }

    private void HandleMatchChatCommand(object? sender, CommandTriggeredEventArgs arguments)
    {
        if (arguments.Player is not TdmPlayer player)
            return;

        if (player.Match == null)
            return;

        string message = string.Join(' ', arguments.Arguments);

        var color = player.Team?.Color ?? Color.White;
        var colorCode = $"#{color.R:X2}{color.G:X2}{color.B:X2}";
        string prefix = $"{colorCode}{player.Name}:#ffffff";
        string fullMessage = $"{prefix} {message}";

        foreach (var teamPlayer in player.Match.Players)
            this.chatBox.OutputTo(teamPlayer, fullMessage, Color.White, true, ChatEchoType.Team, player);

        this.logger?.LogInformation("({match}) {message}", player.Match.Name, $"{player.Name}: {message}");
    }
}
