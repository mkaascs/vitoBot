using System.Reflection;

using Domain.VitoAPI;

using Application.DTO.Commands;
using Application.Abstractions;
using Application.Abstractions.BotCommands;

namespace Application.Services.BotCommands;

public class BotCommandHandler(BotCommandsCollection commandsCollection) : IMessageHandler
{
    public async Task OnGetMessageAsync(IMessageHandlingContext context, CancellationToken cancellationToken = default)
    {
        if (context is not IBotCommandHandlingContext botCommandContext)
            return;

        foreach (IBotCommand botCommand in commandsCollection) 
        {
            BotCommandAttribute? commandAttribute = botCommand.GetType()
                .GetCustomAttribute<BotCommandAttribute>();
            
            if (commandAttribute is null)
                continue;

            if (!string.Equals(commandAttribute.CommandName, botCommandContext.CommandName,
                    StringComparison.CurrentCultureIgnoreCase))
                continue;

            await botCommand.CallAsync(botCommandContext, cancellationToken);
            return;
        }

        await botCommandContext.AnswerAsync(
            new SendMessageCommand("\u274c Такой команды не существует", ContentType.Text),
            cancellationToken);
    }
}