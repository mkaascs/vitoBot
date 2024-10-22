using Domain.Abstractions;
using Domain.VitoAPI;

using Infrastructure.Configuration;
using Infrastructure.Services.VitoAPI.Extensions;

namespace Infrastructure.Services.VitoAPI;

/// <summary>
/// API service designed to interact with chats from VitoAPI
/// </summary>
/// <param name="httpClient">Http client to send http requests to VitoApi</param>
/// <param name="configuration">Configuration of VitoAPI</param>
public class ChatService(HttpClient httpClient, VitoApiConfiguration configuration) : IChatApiService {
    
    public async Task<Response<bool>> RegisterNewChatAsync(Chat chat, CancellationToken cancellationToken = default) 
        => await httpClient.PostAsync(
            CombinePath("chats/"),
            chat,
            cancellationToken);

    public async Task<Response<IEnumerable<Chat>>> GetChatsAsync(CancellationToken cancellationToken = default)
        => await httpClient.GetAsync<IEnumerable<Chat>>(
            CombinePath("chats/"),
            cancellationToken);
    
    
    private string CombinePath(string relativeApiPath)
        => Path.Combine(configuration.DomainName, relativeApiPath);
}