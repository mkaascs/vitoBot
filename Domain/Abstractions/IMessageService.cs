using Domain.API;

namespace Domain.Abstractions;

public interface IMessageService {
    Task<IEnumerable<Message>?> GetMessages(ulong chatId, CancellationToken cancellationToken=default);
    
    Task<Message?> GetRandomMessage(ulong chatId, CancellationToken cancellationToken=default);
}