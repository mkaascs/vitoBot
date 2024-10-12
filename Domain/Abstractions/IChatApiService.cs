using Domain.VitoAPI;

namespace Domain.Abstractions;

/// <summary>
/// API service interface for interacting with <see cref="Chat"/>
/// </summary>
public interface IChatApiService {
    /// <summary>
    /// Method sends POST request to register new instance of <see cref="Chat"/>
    /// </summary>
    /// <param name="chat">An instance of <see cref="Chat"/> containing all required properties to register new chat</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>An instance of response containing a http status code and a bool value that indicates the success of the operation</returns>
    Task<Response<bool>> RegisterNewChatAsync(Chat chat, CancellationToken cancellationToken = default);

    /// <summary>
    /// Method sends GET request to get all registered chats
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>An instance of response containing a http status code and <see cref="IEnumerable{T}"/> of <see cref="Chat"/> instances</returns>
    Task<Response<IEnumerable<Chat>>> GetChatsAsync(CancellationToken cancellationToken = default);
}