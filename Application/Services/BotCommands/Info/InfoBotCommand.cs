using Application.Abstractions.BotCommands;
using Application.DTO;
using Application.DTO.Commands;
using Domain.VitoAPI;

namespace Application.Services.BotCommands.Info;

[BotCommand(CommandName = "info", Description = "Выводит общую информацию о боте")]
public class InfoBotCommand : IBotCommand {
    public Task<IEnumerable<SendMessageCommand>> CallAsync(MessageDto callingMessage, string[] arguments, CancellationToken cancellationToken = default) {
        List<SendMessageCommand> answers = [
            new SendMessageCommand(callingMessage.Chat.Id, "соси пидор", ContentType.Text)
        ];

        return Task.FromResult<IEnumerable<SendMessageCommand>>(answers);
    }
}