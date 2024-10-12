using Domain.API;

namespace Domain.Abstractions;

public interface IMessageService {
    Task<Response<bool>> AddNewMessage(ulong chatId, Message message, CancellationToken cancellationToken = default);
    
    Task<Response<IEnumerable<Message>>> GetMessages(ulong chatId, CancellationToken cancellationToken = default);

    Task<Response<IEnumerable<Message>>> GetMessages(ulong chatId, ContentType type,
        CancellationToken cancellationToken = default);

    Task<Response<Message>> GetRandomMessage(ulong chatId, CancellationToken cancellationToken = default);

    Task<Response<Message>> GetRandomMessage(ulong chatId, ContentType type,
        CancellationToken cancellationToken = default);
}