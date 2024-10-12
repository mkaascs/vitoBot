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

        Response<bool> response =
            await messageService.AddNewMessage(200, new Message("маме свой расскажешь", ContentType.Text));

        Console.WriteLine($"{response.StatusCode} - {response.Content}");
        
        Response<Message> message =
            await messageService.GetRandomMessage(200);
        
        Console.WriteLine($"{message.StatusCode} - {message.Content?.Content} - {message.Content?.Type}");
    }
}