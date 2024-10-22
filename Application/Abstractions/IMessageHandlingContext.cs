using Application.DTO;
using Application.DTO.Commands;

namespace Application.Abstractions;

/// <summary>
/// An interface which represents the context of the handled message
/// </summary>
public interface IMessageHandlingContext {
    /// <summary>
    /// Author of the message
    /// </summary>
    public IUserContext User { get; protected set; }
    
    /// <summary>
    /// A DTO instance of handled message
    /// </summary>
    public MessageDto Message { get; protected set; }

    /// <summary>
    /// An asynchronous method to send an answer to user
    /// </summary>
    /// <param name="command">Instance of <see cref="SendMessageCommand"/> containing required information to send new message</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Returns completed task</returns>
    public Task AnswerAsync(SendMessageCommand command, CancellationToken cancellationToken = default);
}