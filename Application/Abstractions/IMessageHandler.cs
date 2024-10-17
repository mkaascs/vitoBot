using Application.DTO;
using Application.DTO.Commands;

namespace Application.Abstractions;

/// <summary>
/// An interface to handle received messages
/// </summary>
public interface IMessageHandler {
    Task OnGetMessageAsync(MessageDto message, CancellationToken cancellationToken = default);
}