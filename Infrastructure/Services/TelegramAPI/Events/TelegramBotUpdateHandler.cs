using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

using Infrastructure.Services.TelegramAPI.Events.Interfaces;

namespace Infrastructure.Services.TelegramAPI.Events;

public class TelegramBotUpdateHandler : IUpdateHandler, IAsyncObservable<Message?> {
    private readonly List<IAsyncObserver<Message?>> _messageHandlers = [];
    
    public IDisposable Subscribe(IAsyncObserver<Message?> observer) {
        _messageHandlers.Add(observer ?? throw new ArgumentNullException(nameof(observer)));
        return new UnsubscribeToken<Message?>(observer, _messageHandlers);
    }
    
    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken) {
        IList<Task> onNextTasks = new List<Task>(_messageHandlers.Count);
        foreach (IAsyncObserver<Message?> messageHandler in _messageHandlers)
            onNextTasks.Add(messageHandler.OnNextAsync(update.Message, cancellationToken));

        await Task.WhenAll(onNextTasks);
    }

    public Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        => Task.CompletedTask;
}