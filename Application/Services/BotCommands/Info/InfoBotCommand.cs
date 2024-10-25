using Domain.VitoAPI;

using Application.DTO.Commands;
using Application.Abstractions.BotCommands;

namespace Application.Services.BotCommands.Info;

[BotCommand(CommandName = "info", Description = "Выводит общую информацию о боте")]
public class InfoBotCommand : IBotCommand
{
    public async Task CallAsync(IBotCommandHandlingContext context, CancellationToken cancellationToken = default)
    {
        await context.AnswerAsync(new SendMessageCommand("хуй соси", ContentType.Text), cancellationToken);
    }
}