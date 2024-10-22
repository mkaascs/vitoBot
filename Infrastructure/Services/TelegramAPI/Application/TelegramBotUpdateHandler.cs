using Microsoft.Extensions.Logging;

using Application.Abstractions;

using Infrastructure.Services.TelegramAPI.Extensions;

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

        IMessageHandlingContext context = await GetMessageHandlingContextAsync(botClient, update.Message, cancellationToken);
        
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

    private async Task<IMessageHandlingContext> GetMessageHandlingContextAsync(ITelegramBotClient botClient, Message message,
        CancellationToken cancellationToken = default) {
        
        string? textMessage = message.Text;
        TelegramUserContext userContext = await botClient.GetUserContextFrom(message, cancellationToken);

        if (textMessage != null && textMessage.StartsWith(CommandSymbol))
            return new TelegramBotCommandHandlingContext(
                userContext,
                textMessage[1..],
                message,
                messageSender);

        return new TelegramMessageHandlingContext(
            userContext,
            message,
            messageSender);
    }
}