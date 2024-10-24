namespace Application.Abstractions;

/// <summary>
/// An interface to handle received messages
/// </summary>
public interface IMessageHandler
{
    /// <summary>
    /// Executes when a new message was received
    /// </summary>
    /// <param name="context">Context of message handling</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Returns completed task</returns>
    Task OnGetMessageAsync(IMessageHandlingContext context, CancellationToken cancellationToken = default);
}