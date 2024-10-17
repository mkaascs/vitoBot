using System.Collections;

using Application.Abstractions.BotCommands;
using Application.Services.BotCommands.Help;
using Application.Services.BotCommands.Info;
using Application.Services.BotCommands.Settings;
using Domain.Abstractions;

namespace Application.Services.BotCommands;

public class BotCommandsCollection : IEnumerable<IBotCommand> {
    private readonly IEnumerable<IBotCommand> _botCommands;

    public BotCommandsCollection(IUserSettingsRepository repository) {
        _botCommands = new IBotCommand[] {
            new HelpBotCommand(this),
            new InfoBotCommand(),
            new SettingsBotCommand(repository)
        };
    }

    public IEnumerator<IBotCommand> GetEnumerator() {
        foreach (IBotCommand botCommand in _botCommands)
            yield return botCommand;
    }

    IEnumerator IEnumerable.GetEnumerator() 
        => GetEnumerator();
}