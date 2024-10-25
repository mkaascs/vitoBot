using Domain.VitoAPI;

namespace Domain.Abstractions;

public interface IChatApiService
{
    ValueTask<bool> RegisterNewChatAsync(Chat chat, CancellationToken cancellationToken = default);
    
    ValueTask<IEnumerable<Chat>?> GetChatsAsync(CancellationToken cancellationToken = default);

    ValueTask<Chat?> GetChatByIdAsync(ulong chatId, CancellationToken cancellationToken = default);
}