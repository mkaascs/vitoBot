using Microsoft.Extensions.Logging;

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
public class ChatService(
    HttpClient httpClient,
    VitoApiConfiguration configuration,
    ILogger<ChatService> logger) : IChatApiService
{
    public async Task<Response> RegisterNewChatAsync(Chat chat, CancellationToken cancellationToken = default)
    {
        Response response = await httpClient
            .PostAsync(chat, CombinePath("chats/"), cancellationToken);

        logger.LogResponse(response, "POST", "register new chat");
        return response;
    }

    public async Task<Response<IEnumerable<Chat>>> GetChatsAsync(CancellationToken cancellationToken = default)
    {
        Response<IEnumerable<Chat>> response = await httpClient
            .GetAsync<IEnumerable<Chat>>(CombinePath("chats/"), cancellationToken);
        
        logger.LogResponse(response, "GET", "get all registered chats");
        return response;
    }

    public async Task<Response<Chat>> GetChatByIdAsync(ulong chatId, CancellationToken cancellationToken = default)
    {
        Response<Chat> response = await httpClient
            .GetAsync<Chat>(CombinePath(chatId.ToString()), cancellationToken);
        
        logger.LogResponse(response, "GET", "get chat by id");
        return response;
    }

    private string CombinePath(string relativeApiPath)
    {
        return Path.Combine(configuration.DomainName, relativeApiPath);
    }
}