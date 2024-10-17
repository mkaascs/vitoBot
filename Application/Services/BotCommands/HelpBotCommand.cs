using System.Reflection;

using Domain.VitoAPI;

using Application.Abstractions.BotCommands;
using Application.DTO;
using Application.DTO.Commands;

namespace Application.Services.BotCommands;

[BotCommand(CommandName = "help", Description = "Displays all bot commands")]
public class HelpBotCommand(BotCommandsCollection commandsCollection) : IBotCommand {
    public Task<IEnumerable<SendMessageCommand>> CallAsync(MessageDto callingMessage, string[] arguments, CancellationToken cancellationToken = default) {
        string content = commandsCollection
            .Select(command => command.GetType()
                .GetCustomAttribute<BotCommandAttribute>())
            .OfType<BotCommandAttribute>()
            .Aggregate("vitoBot has the following commands:\n\n", (current, commandAttribute) => current + $"   /{commandAttribute.CommandName.ToLower()} - {commandAttribute.Description.ToLower()}\n");

        return Task.FromResult<IEnumerable<SendMessageCommand>>(new[] {
            new SendMessageCommand(callingMessage.Chat.Id, content, ContentType.Text)
        });
    }
}