using System.Reflection;

using Domain.VitoAPI;

using Application.Abstractions;
using Application.Abstractions.BotCommands;
using Application.DTO;
using Application.DTO.Commands;

namespace Application.Services.BotCommands;

public class BotCommandHandler(char commandSymbol, IMessageSender messageSender, BotCommandsCollection commandsCollection) {
    public async Task<bool> TryExecuteCommand(MessageDto message, CancellationToken cancellationToken = default) {
        if (string.IsNullOrWhiteSpace(message.Content))
            return false;

        string[] splitMessage = message.Content.Trim().Split(' ');
        if (splitMessage[0][0] != commandSymbol)
            return false;

        foreach (IBotCommand command in commandsCollection) {
            BotCommandAttribute? commandAttribute = command
                .GetType()
                .GetCustomAttribute<BotCommandAttribute>();

            if (commandAttribute is null)
                continue;
            
            if (!IsCommand(commandAttribute, splitMessage[0]))
                continue;

            IEnumerable<SendMessageCommand> answers = await command
                .CallAsync(message, splitMessage.Skip(1).ToArray(), cancellationToken);

            foreach (SendMessageCommand answer in answers)
                await messageSender.SendMessageAsync(answer, cancellationToken);

            return true;
        }

        await messageSender.SendMessageAsync(
            new SendMessageCommand(
                message.Chat.Id,
                "\u274c there is no such command",
                ContentType.Text),
            cancellationToken);

        return true;
    }

    private bool IsCommand(BotCommandAttribute commandAttribute, string message)
        => message.ToLower() == $"{commandSymbol}{commandAttribute.CommandName.ToLower()}";
}