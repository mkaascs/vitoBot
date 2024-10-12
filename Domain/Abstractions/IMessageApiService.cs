using Domain.VitoAPI;

namespace Domain.Abstractions;

/// <summary>
/// API service interface for interacting with <see cref="Message"/>
/// </summary>
public interface IMessageApiService {
    /// <summary>
    /// Method sends POST request to add new instance of <see cref="Message"/> in the specific chat
    /// </summary>
    /// <param name="chatId">Unique id of chat to add new message</param>
    /// <param name="message">Instance of <see cref="Message"/> containing all required properties to add new message</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>An instance of response containing a http status code and a bool value that indicates the success of the operation</returns>
    Task<Response<bool>> AddNewMessageAsync(ulong chatId, Message message, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Method sends GET request to get all messages in the specific chat
    /// </summary>
    /// <param name="chatId">Unique id of chat from where you need to get messages</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>An instance of response containing a http status code and <see cref="IEnumerable{T}"/> of <see cref="Message"/> instances</returns>
    Task<Response<IEnumerable<Message>>> GetMessagesAsync(ulong chatId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Method sends GET request to get all messages of a certain type in the specific chat
    /// </summary>
    /// <param name="chatId">Unique id of chat from where you need to get messages</param>
    /// <param name="type">Type of messages you need to get</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>An instance of response containing a http status code and <see cref="IEnumerable{T}"/> of <see cref="Message"/> instances</returns>
    Task<Response<IEnumerable<Message>>> GetMessagesAsync(ulong chatId, ContentType type,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Method sends GET request to get a random message in the specific chat
    /// </summary>
    /// <param name="chatId">Unique id of chat from where you need to get a message</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>An instance of response containing a http status code and a random instance of <see cref="Message"/></returns>
    Task<Response<Message>> GetRandomMessageAsync(ulong chatId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Method sends GET request to get a random message of a certain type in the specific chat
    /// </summary>
    /// <param name="chatId">Unique id of chat from where you need to get a message</param>
    /// <param name="type">Type of message you need to get</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>An instance of response containing a http status code and a random instance of <see cref="Message"/></returns>
    Task<Response<Message>> GetRandomMessageAsync(ulong chatId, ContentType type,
        CancellationToken cancellationToken = default);
}