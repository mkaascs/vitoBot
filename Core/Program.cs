using Domain.Abstractions;
using Domain.API;
using Infrastructure.Configuration;
using Infrastructure.Services.VitoAPI;

namespace Core;

internal class Program {
    private static async Task Main(string[] args) {
        VitoApiConfiguration apiConfiguration = new("http://localhost:5000");

        IMessageService messageService
            = new MessageService(new HttpClient(), apiConfiguration);

        Message? message = await messageService.GetRandomMessage(200);
        Console.WriteLine(message?.Content);
    }
}