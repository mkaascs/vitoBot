using Domain.Abstractions;
using Domain.VitoAPI;

using Infrastructure.Configuration;
using Infrastructure.Services.VitoAPI.Extensions;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services.VitoAPI;

/// <summary>
/// API service designed to interact with messages from VitoAPI
/// </summary>
/// <param name="httpClient">Http client to send http requests to VitoApi</param>
/// <param name="configuration">Configuration of VitoAPI</param>
public class MessageService(
    HttpClient httpClient,
    VitoApiConfiguration configuration,
    ILogger<MessageService> logger) : IMessageApiService {

    public async Task<Response> AddNewMessageAsync(
        ulong chatId,
        Message message,
        CancellationToken cancellationToken = default)
    {
        Response response = await httpClient
            .PostAsync(message, CombinePath($"chats/{chatId}"), cancellationToken);
        
        logger.LogResponse(response, "POST", "add new message");
        return response;
    }

    public async Task<Response<IEnumerable<Message>>> GetMessagesAsync(
        ulong chatId,
        CancellationToken cancellationToken = default)
    {
        Response<IEnumerable<Message>> response = await httpClient
            .GetAsync<IEnumerable<Message>>(CombinePath($"chats/{chatId}"), cancellationToken);
        
        logger.LogResponse(response, "GET", "get all messages from chat");
        return response;
    }

    public async Task<Response<Message>> GetRandomMessageAsync(
        ulong chatId,
        CancellationToken cancellationToken = default)
    {
        Response<Message> response = await httpClient
            .GetAsync<Message>(CombinePath($"chats/{chatId}/random"), cancellationToken);
        
        logger.LogResponse(response, "GET", "get random message from chat");
        return response;
    }
    
    private string CombinePath(string relativeApiPath)
    {
        return Path.Combine(configuration.DomainName, relativeApiPath);
    }
}