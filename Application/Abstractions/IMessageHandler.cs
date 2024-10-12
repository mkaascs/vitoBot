using Application.DTO;

namespace Application.Abstractions;

/// <summary>
/// An interface to handle received messages
/// </summary>
public interface IMessageHandler {
    /// <summary>
    /// Called when a new message is received
    /// </summary>
    /// <param name="message">A <see cref="MessageDto"/> model which contains all information about the received message</param>
    void OnGetMessage(MessageDto message);
}