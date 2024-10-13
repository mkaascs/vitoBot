namespace Infrastructure.Services.TelegramAPI.Events.Interfaces;

public interface IAsyncObserver<in T> {
    Task OnNextAsync(T value);

    Task OnCompletedAsync();

    Task OnErrorAsync(Exception exception);
}