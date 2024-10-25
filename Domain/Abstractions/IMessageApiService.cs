using Domain.VitoAPI;

namespace Domain.Abstractions;

public interface IMessageApiService
{
    ValueTask<bool> AddNewMessageAsync(ulong chatId, Message message, CancellationToken cancellationToken = default);
    
    ValueTask<IEnumerable<Message>?> GetMessagesAsync(ulong chatId, CancellationToken cancellationToken = default);
    
    ValueTask<Message?> GetRandomMessageAsync(ulong chatId, CancellationToken cancellationToken = default);
}