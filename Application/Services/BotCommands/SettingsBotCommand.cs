using Application.Abstractions.BotCommands;
using Application.DTO;
using Application.DTO.Commands;

namespace Application.Services.BotCommands;

[BotCommand(CommandName = "settings", Description = "Allows you to change the chances of sending and saving messages")]
public class SettingsBotCommand : IBotCommand {
    public Task<IEnumerable<SendMessageCommand>> CallAsync(MessageDto callingMessage, string[] arguments, CancellationToken cancellationToken = default) {
        throw new NotImplementedException();
    }
}