using Domain.Abstractions;
using Domain.API;

using Infrastructure.Configuration;
using Infrastructure.Services.VitoAPI.Extensions;

namespace Infrastructure.Services.VitoAPI;

public class MessageService(HttpClient httpClient, VitoApiConfiguration configuration) : IMessageService {
    public async Task<Response<bool>> AddNewMessage(ulong chatId, Message message,
        CancellationToken cancellationToken = default) 
        => await httpClient.PostAsync<Message>(
            CombinePath($"chats/{chatId}"),
            message,
            cancellationToken);

    public async Task<Response<IEnumerable<Message>>> GetMessages(ulong chatId,
        CancellationToken cancellationToken = default)
        => await httpClient.GetAsync<IEnumerable<Message>>(
            CombinePath($"chats/{chatId}"),
            cancellationToken);

    public async Task<Response<IEnumerable<Message>>> GetMessages(ulong chatId, ContentType type,
        CancellationToken cancellationToken = default)
        => await httpClient.GetAsync<IEnumerable<Message>>(
            CombinePath($"chats/{chatId}/{type}"),
            cancellationToken);

    public async Task<Response<Message>> GetRandomMessage(ulong chatId, CancellationToken cancellationToken = default)
        => await httpClient.GetAsync<Message>(
            CombinePath($"chats/{chatId}/random"),
            cancellationToken);
    
    public async Task<Response<Message>> GetRandomMessage(ulong chatId, ContentType type,
        CancellationToken cancellationToken = default)
        => await httpClient.GetAsync<Message>(
            CombinePath($"chats/{chatId}/{type}/random"),
            cancellationToken);

    private string CombinePath(string relativeApiPath)
        => Path.Combine(configuration.DomainName, relativeApiPath);
}