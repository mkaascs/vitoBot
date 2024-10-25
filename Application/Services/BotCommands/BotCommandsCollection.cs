using Microsoft.Extensions.DependencyInjection;

using System.Collections;

using Domain.Abstractions;

using Application.Abstractions.BotCommands;
using Application.Services.BotCommands.Help;
using Application.Services.BotCommands.Info;
using Application.Services.BotCommands.Settings;

namespace Application.Services.BotCommands;

public class BotCommandsCollection : IEnumerable<IBotCommand> 
{
    private readonly IEnumerable<IBotCommand> _botCommands;

    public BotCommandsCollection(IServiceProvider serviceProvider) 
    {
        _botCommands = new IBotCommand[] 
        {
            new HelpBotCommand(this),
            new InfoBotCommand(),
            new SettingsBotCommand(
                serviceProvider.GetRequiredService<IUserSettingsRepository>())
        };
    }

    public IEnumerator<IBotCommand> GetEnumerator()
    {
        foreach (IBotCommand botCommand in _botCommands)
            yield return botCommand;
    }

    IEnumerator IEnumerable.GetEnumerator() 
        => GetEnumerator();
}