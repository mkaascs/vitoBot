namespace Application.Abstractions;

/// <summary>
/// An interface to handle received messages
/// </summary>
public interface IMessageHandler {
    Task OnGetMessageAsync(IMessageHandlingContext context, CancellationToken cancellationToken = default);
}