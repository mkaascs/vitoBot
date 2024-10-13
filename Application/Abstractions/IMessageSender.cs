using Application.DTO.Commands;

namespace Application.Abstractions;

/// <summary>
/// An interface to send messages
/// </summary>
public interface IMessageSender {
    /// <summary>
    /// Send a message asynchronously
    /// </summary>
    /// <param name="command">An instance of <see cref="SendMessageCommand"/> containing all required properties to send a message</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Completed task</returns>
    Task SendMessageAsync(SendMessageCommand command, CancellationToken cancellationToken = default);
}