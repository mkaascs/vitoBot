using Domain.API;
using Domain.Abstractions;

using Infrastructure.Configuration;
using Infrastructure.Services.VitoAPI.Extensions;

namespace Infrastructure.Services.VitoAPI;

/// <summary>
/// API service designed to interact with messages from VitoAPI
/// </summary>
/// <param name="httpClient">Http client to send http requests to VitoApi</param>
/// <param name="configuration">Configuration of VitoAPI</param>
public class MessageService(HttpClient httpClient, VitoApiConfiguration configuration) : IMessageApiService {
    
    public async Task<Response<bool>> AddNewMessageAsync(ulong chatId, Message message,
        CancellationToken cancellationToken = default) 
        => await httpClient.PostAsync(
            CombinePath($"chats/{chatId}"),
            message,
            cancellationToken);

    
    public async Task<Response<IEnumerable<Message>>> GetMessagesAsync(ulong chatId,
        CancellationToken cancellationToken = default)
        => await httpClient.GetAsync<IEnumerable<Message>>(
            CombinePath($"chats/{chatId}"),
            cancellationToken);

    
    public async Task<Response<IEnumerable<Message>>> GetMessagesAsync(ulong chatId, ContentType type,
        CancellationToken cancellationToken = default)
        => await httpClient.GetAsync<IEnumerable<Message>>(
            CombinePath($"chats/{chatId}/{type}"),
            cancellationToken);

    
    public async Task<Response<Message>> GetRandomMessageAsync(ulong chatId, CancellationToken cancellationToken = default)
        => await httpClient.GetAsync<Message>(
            CombinePath($"chats/{chatId}/random"),
            cancellationToken);
    
    
    public async Task<Response<Message>> GetRandomMessageAsync(ulong chatId, ContentType type,
        CancellationToken cancellationToken = default)
        => await httpClient.GetAsync<Message>(
            CombinePath($"chats/{chatId}/{type}/random"),
            cancellationToken);

    
    private string CombinePath(string relativeApiPath)
        => Path.Combine(configuration.DomainName, relativeApiPath);
}