using System.Reflection;

using Domain.VitoAPI;

using Application.DTO.Commands;
using Application.Abstractions.BotCommands;

namespace Application.Services.BotCommands.Help;

[BotCommand(CommandName = "help", Description = "Выводит все существующие команды бота")]
public class HelpBotCommand(BotCommandsCollection commandsCollection) : IBotCommand {
    public async Task CallAsync(IBotCommandHandlingContext context, CancellationToken cancellationToken = default) {
        string content = commandsCollection
            .Select(command => command.GetType()
                .GetCustomAttribute<BotCommandAttribute>())
            .OfType<BotCommandAttribute>()
            .Aggregate("ℹ\ufe0f VitoBot имеет следующие команды:\n\n", (current, commandAttribute) => current + $"/{commandAttribute.CommandName.ToLower()} - {commandAttribute.Description.ToLower()}\n");

        await context.AnswerAsync(new SendMessageCommand(content, ContentType.Text), cancellationToken);
    }
}