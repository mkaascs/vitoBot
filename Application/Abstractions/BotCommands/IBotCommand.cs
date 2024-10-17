using Application.DTO;
using Application.DTO.Commands;

namespace Application.Abstractions.BotCommands;

public interface IBotCommand {
    Task<IEnumerable<SendMessageCommand>> CallAsync(MessageDto callingMessage, string[] arguments, CancellationToken cancellationToken = default);
}