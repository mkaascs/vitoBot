using Domain.VitoAPI;

using Application.DTO;
using Application.DTO.Commands;
using Application.Abstractions.BotCommands;

namespace Application.Services.BotCommands;

[BotCommand(CommandName = "info", Description = "Displays general information about the bot")]
public class InfoBotCommand : IBotCommand {
    public Task<IEnumerable<SendMessageCommand>> CallAsync(MessageDto callingMessage, string[] arguments, CancellationToken cancellationToken = default) {
        List<SendMessageCommand> answers = [
            new SendMessageCommand(callingMessage.Chat.Id, "соси пидор", ContentType.Text)
        ];

        return Task.FromResult<IEnumerable<SendMessageCommand>>(answers);
    }
}