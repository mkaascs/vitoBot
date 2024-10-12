using Domain.API;
using Infrastructure.Configuration;
using Infrastructure.Services.VitoAPI;

namespace Core;

internal class Program {
    private static async Task Main(string[] args) {
        HttpClient httpClient = new();
        VitoApiConfiguration apiConfiguration = new("http://localhost:5000");

        MessageService messageApiService
            = new MessageService(httpClient, apiConfiguration);
        
        Response<Message> message =
            await messageApiService.GetRandomMessageAsync(200);
        
        Console.WriteLine($"{message.StatusCode} - {message.Content?.Content} - {message.Content?.Type}");
    }
}