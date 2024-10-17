using System.Reflection;

using Domain.VitoAPI;

using Application.DTO.Commands;
using Application.Abstractions.BotCommands;

using Infrastructure.Services.TelegramAPI.Application.Extensions;

using Message = Telegram.Bot.Types.Message;

namespace Infrastructure.Services.TelegramAPI.Application;

public class BotCommandHandler(IEnumerable<IBotCommand> registeredBotCommands) {
    public async Task<IEnumerable<SendMessageCommand>> ExecuteCommandIfExists(Message message, CancellationToken cancellationToken = default) {
        if (string.IsNullOrWhiteSpace(message.Text))
            return [];

        string[] splitMessage = message.Text.Trim().Split(' ');

        if (!IsCommandSymbol(splitMessage[0][0]))
            return [];

        foreach (IBotCommand command in registeredBotCommands) {
            BotCommandAttribute? botCommandAttribute = command
                .GetType()
                .GetCustomAttribute<BotCommandAttribute>();
            
            if (botCommandAttribute is null)
                continue;

            if (IsCommand(botCommandAttribute, splitMessage[0]))
                return await command.CallAsync(
                    message.ToDto(),
                    splitMessage.Skip(1).ToArray(),
                    cancellationToken);
        }

        return new[] {
            new SendMessageCommand(
                (ulong)message.Chat.Id, 
                "ERROR: there is no such command",
                ContentType.Text)
        };
    }

    private bool IsCommandSymbol(char symbol)
        => symbol is '/' or '\\';

    private bool IsCommand(BotCommandAttribute botCommand, string message)
        => message == $"/{botCommand.CommandName.ToLower()}"
           || message == $"\\{botCommand.CommandName.ToLower()}";
}