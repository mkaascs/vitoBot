using Application.Abstractions.BotCommands;

using Telegram.Bot.Types;

namespace Infrastructure.Services.TelegramAPI.Application;

public class TelegramBotCommandHandlingContext : TelegramMessageHandlingContext, IBotCommandHandlingContext {
    public TelegramBotCommandHandlingContext(TelegramUserContext userContext, string fullCommand, Message message, TelegramBotMessageSender messageSender)
        : base(userContext, message, messageSender) {
        
        ArgumentException.ThrowIfNullOrWhiteSpace(fullCommand);

        string[] splitCommand = fullCommand.Split(' ');
        CommandName = splitCommand[0];
        Arguments = splitCommand.Skip(1).ToArray();
    }
    
    public string CommandName { get; set; }
    
    public string[] Arguments { get; set; }
}