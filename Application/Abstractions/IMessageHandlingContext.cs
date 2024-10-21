using Application.DTO;
using Application.DTO.Commands;

namespace Application.Abstractions;

public interface IMessageHandlingContext {
    public MessageDto Message { get; protected set; }

    public Task AnswerAsync(SendMessageCommand command, CancellationToken cancellationToken = default);
}