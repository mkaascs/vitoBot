using Microsoft.Extensions.Logging;

using Domain.Abstractions;
using Domain.VitoAPI;

using Infrastructure.Configuration;
using Infrastructure.Services.VitoAPI.Extensions;
using Infrastructure.Services.VitoAPI.Responses;

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
    public async ValueTask<bool> RegisterNewChatAsync(Chat chat, CancellationToken cancellationToken = default)
    {
        HttpResponseMessage httpResponse = await httpClient
            .PostAsync(chat, CombinePath("chats/"), cancellationToken);

        Response response = await httpResponse.ToResponse(cancellationToken);
        logger.LogResponse(response, nameof(RegisterNewChatAsync));
        
        return response.IsSuccess;
    }

    public async ValueTask<IEnumerable<Chat>?> GetChatsAsync(CancellationToken cancellationToken = default)
    {
        HttpResponseMessage httpResponse = await httpClient
            .GetAsync(CombinePath("chats/"), cancellationToken);

        Response<IEnumerable<Chat>> response = await httpResponse
            .ToResponse<IEnumerable<Chat>>(cancellationToken);
        
        logger.LogResponse(response, nameof(GetChatsAsync));
        return response.Content;
    }

    public async ValueTask<Chat?> GetChatByIdAsync(ulong chatId, CancellationToken cancellationToken = default)
    {
        HttpResponseMessage httpResponse = await httpClient
            .GetAsync(CombinePath($"chats/{chatId}"), cancellationToken);

        Response<Chat> response = await httpResponse.ToResponse<Chat>(cancellationToken);
        
        logger.LogResponse(response, nameof(GetChatByIdAsync));
        return response.Content;
    }

    private string CombinePath(string relativeApiPath)
    {
        return configuration.Protocol + Path.Combine(configuration.DomainName, relativeApiPath);
    }
}