namespace Infrastructure.Services.TelegramAPI.Events.Interfaces;

public interface IAsyncObserver<in T> {
    Task OnNextAsync(T value, CancellationToken cancellationToken = default);

    Task OnCompletedAsync(CancellationToken cancellationToken = default);

    Task OnErrorAsync(Exception exception, CancellationToken cancellationToken = default);
}