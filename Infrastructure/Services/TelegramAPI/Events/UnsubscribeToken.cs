using Infrastructure.Services.TelegramAPI.Events.Interfaces;

namespace Infrastructure.Services.TelegramAPI.Events;

public class UnsubscribeToken<T>(IAsyncObserver<T> subscribedObserver, IList<IAsyncObserver<T>> observers) : IDisposable {
    public void Dispose()
        => observers.Remove(subscribedObserver);
}