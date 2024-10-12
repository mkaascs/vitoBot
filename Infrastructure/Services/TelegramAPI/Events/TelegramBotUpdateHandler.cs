using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace Infrastructure.Services.TelegramAPI.Events;

public class TelegramBotUpdateHandler : IUpdateHandler, IObservable<Message?> {
    private readonly List<IObserver<Message?>> _messageHandlers = [];
    
    public IDisposable Subscribe(IObserver<Message?> observer) {
        _messageHandlers.Add(observer ?? throw new ArgumentNullException(nameof(observer)));
        return new UnsubscribeToken<Message?>(observer, _messageHandlers);
    }
    
    public Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken) {
        foreach (IObserver<Message?> messageHandler in _messageHandlers)
            messageHandler.OnNext(update.Message);

        return Task.CompletedTask;
    }

    public Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        => Task.CompletedTask;
}