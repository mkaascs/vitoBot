using System.Collections;

using Application.Abstractions.BotCommands;

namespace Application.Services.BotCommands;

public class BotCommandsCollection : IEnumerable<IBotCommand> {
    private readonly IEnumerable<IBotCommand> _botCommands = new[] {
        new InfoBotCommand()
    };

    public IEnumerator<IBotCommand> GetEnumerator() {
        foreach (IBotCommand botCommand in _botCommands)
            yield return botCommand;
    }

    IEnumerator IEnumerable.GetEnumerator() {
        return GetEnumerator();
    }
}