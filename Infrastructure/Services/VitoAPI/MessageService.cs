using Microsoft.Extensions.Logging;

using Domain.Abstractions;
using Domain.VitoAPI;

using Infrastructure.Configuration;
using Infrastructure.Services.VitoAPI.Extensions;
using Infrastructure.Services.VitoAPI.Responses;

namespace Infrastructure.Services.VitoAPI;

/// <summary>
/// API service designed to interact with messages from VitoAPI
/// </summary>
/// <param name="httpClient">Http client to send http requests to VitoApi</param>
/// <param name="configuration">Configuration of VitoAPI</param>
public class MessageService(
    HttpClient httpClient,
    VitoApiConfiguration configuration,
    ILogger<MessageService> logger) : IMessageApiService 
{
    public async ValueTask<bool> AddNewMessageAsync(
        ulong chatId,
        Message message,
        CancellationToken cancellationToken = default)
    {
        HttpResponseMessage httpResponse = await httpClient
            .PostAsync(message, CombinePath($"chats/{chatId}/messages"), cancellationToken);

        Response response = await httpResponse.ToResponse(cancellationToken);
        logger.LogResponse(response, nameof(AddNewMessageAsync));
        
        return response.IsSuccess;
    }

    public async ValueTask<IEnumerable<Message>?> GetMessagesAsync(
        ulong chatId,
        CancellationToken cancellationToken = default)
    {
        HttpResponseMessage httpResponse = await httpClient
            .GetAsync(CombinePath($"chats/{chatId}/messages"), cancellationToken);

        Response<IEnumerable<Message>> response = await httpResponse
            .ToResponse<IEnumerable<Message>>(cancellationToken);
        
        logger.LogResponse(response, nameof(GetMessagesAsync));
        
        return response.Content;
    }

    public async ValueTask<Message?> GetRandomMessageAsync(
        ulong chatId,
        CancellationToken cancellationToken = default)
    {
        HttpResponseMessage httpResponse = await httpClient
            .GetAsync(CombinePath($"chats/{chatId}/messages/random"), cancellationToken);

        Response<Message> response = await httpResponse.ToResponse<Message>(cancellationToken);
        logger.LogResponse(response, nameof(GetRandomMessageAsync));
        
        return response.Content;
    }
    
    private string CombinePath(string relativeApiPath)
    {
        return configuration.Protocol + Path.Combine(configuration.DomainName, relativeApiPath);
    }
}