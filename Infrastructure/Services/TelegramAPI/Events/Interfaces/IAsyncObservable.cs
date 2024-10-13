namespace Infrastructure.Services.TelegramAPI.Events.Interfaces;

public interface IAsyncObservable<out T> {
    public IDisposable Subscribe(IAsyncObserver<T> observer);
}