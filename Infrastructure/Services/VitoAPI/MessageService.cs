using System.Net;
using Newtonsoft.Json;

using Domain.Abstractions;
using Domain.API;

using Infrastructure.Configuration;

namespace Infrastructure.Services.VitoAPI;

public class MessageService(HttpClient httpClient, VitoApiConfiguration configuration) : IMessageService {
    public async Task<IEnumerable<Message>?> GetMessages(ulong chatId, CancellationToken cancellationToken=default) {
        HttpResponseMessage responseMessage = await httpClient
            .GetAsync($"{configuration.DomainName}/chats/{chatId}", cancellationToken);

        if (responseMessage.StatusCode != HttpStatusCode.OK)
            return null;

        IEnumerable<Message>? messages =
            JsonConvert.DeserializeObject<IEnumerable<Message>>
                (await responseMessage.Content.ReadAsStringAsync(cancellationToken));

        return messages;
    }

    public async Task<Message?> GetRandomMessage(ulong chatId, CancellationToken cancellationToken=default) {
        HttpResponseMessage responseMessage = await httpClient
            .GetAsync($"{configuration.DomainName}/chats/{chatId}/random", cancellationToken);

        if (responseMessage.StatusCode != HttpStatusCode.OK)
            return null;

        Message? message = JsonConvert.DeserializeObject<Message>(
            await responseMessage.Content.ReadAsStringAsync(cancellationToken));

        return message;
    }
}