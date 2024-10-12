namespace Infrastructure.Services.TelegramAPI.Events;

public class UnsubscribeToken<T>(IObserver<T> subscribedObserver, IList<IObserver<T>> observers) : IDisposable {
    public void Dispose()
        => observers.Remove(subscribedObserver);
}