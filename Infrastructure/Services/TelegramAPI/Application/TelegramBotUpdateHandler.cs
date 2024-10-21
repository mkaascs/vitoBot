using Microsoft.Extensions.Logging;

using Application.Abstractions;

using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace Infrastructure.Services.TelegramAPI.Application;

public class TelegramBotUpdateHandler(TelegramBotMessageSender messageSender, ILogger logger) : IUpdateHandler {
    private const char CommandSymbol = '/';
    private readonly List<IMessageHandler> _messageHandlers = [];
    
    public void Subscribe(IMessageHandler messageHandler) {
        ArgumentNullException.ThrowIfNull(messageHandler);
        _messageHandlers.Add(messageHandler);
    }
    
    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken) {
        if (update.Message is null)
            return;
        
        logger.LogInformation("A message received: {from}: {text}",
            update.Message.Chat.Title ?? update.Message.From?.FirstName, update.Message.Text);

        string? trimmedTextMessage = update.Message.Text;

        IMessageHandlingContext context = trimmedTextMessage != null && trimmedTextMessage.StartsWith(CommandSymbol)
            ? new TelegramBotCommandHandlingContext(trimmedTextMessage[1..], update.Message, messageSender)
            : new TelegramMessageHandlingContext(update.Message, messageSender);
        
        
        IList<Task> onGetTasks = new List<Task>(_messageHandlers.Count);
        foreach (IMessageHandler messageHandler in _messageHandlers)
            onGetTasks.Add(messageHandler.OnGetMessageAsync(context, cancellationToken));
        
        await Task.WhenAll(onGetTasks);
    }

    public Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception,
        CancellationToken cancellationToken) {
        
        logger.LogError(exception, "An polling error occured");
        return Task.CompletedTask;
    }
}