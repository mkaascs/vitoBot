using Microsoft.Extensions.Logging;

using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

using Application.Abstractions;

using Infrastructure.Services.TelegramAPI.Extensions;

namespace Infrastructure.Services.TelegramAPI;

public class TelegramBotUpdateHandler(ILogger<TelegramBotUpdateHandler> logger) : IUpdateHandler {
    private readonly List<IMessageHandler> _messageHandlers = [];
    
    public void Subscribe(IMessageHandler messageHandler) {
        ArgumentNullException.ThrowIfNull(messageHandler);
        _messageHandlers.Add(messageHandler);
    }
    
    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken) {
        if (update.Message is null || string.IsNullOrWhiteSpace(update.Message.Text))
            return;
        
        logger.LogInformation("A message received: {from}: {text}",
            update.Message.From?.FirstName, update.Message.Text);
        
        IList<Task> onGetTasks = new List<Task>(_messageHandlers.Count);
        foreach (IMessageHandler messageHandler in _messageHandlers)
            onGetTasks.Add(messageHandler.OnGetMessageAsync(update.Message.ToDto(), cancellationToken));
        
        await Task.WhenAll(onGetTasks);
    }

    public Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception,
        CancellationToken cancellationToken) {
        
        logger.LogError(exception, "An polling error occured");
        return Task.CompletedTask;
    }
}