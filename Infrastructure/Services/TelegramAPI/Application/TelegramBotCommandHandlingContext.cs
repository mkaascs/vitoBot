using Application.Abstractions.BotCommands;

using Telegram.Bot.Types;

namespace Infrastructure.Services.TelegramAPI.Application;

public class TelegramBotCommandHandlingContext 
    : TelegramMessageHandlingContext, IBotCommandHandlingContext
{
    public TelegramBotCommandHandlingContext(
        string fullCommand,
        Message message, 
        TelegramUserContext userContext,
        TelegramBotMessageSender messageSender)
        : base(message, userContext, messageSender)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(fullCommand);

        string[] splitCommand = fullCommand.Split(' ');
        CommandName = splitCommand[0];
        Arguments = splitCommand.Skip(1).ToArray();
    }
    
    public string CommandName { get; set; }
    
    public string[] Arguments { get; set; }
}