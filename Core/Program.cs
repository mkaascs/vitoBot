using Application.Services;

using Infrastructure.Configuration;
using Infrastructure.Services.TelegramAPI;
using Infrastructure.Services.TelegramAPI.Events;

namespace Core;

internal class Program {
    private const string ApiKey = "5941206869:AAFKW9x9hNOVh5I9ez-XMgJ8YK0KYn_0GPk";
    private static void Main(string[] args) {
        BotClient botBotClient = new BotClient(new TelegramApiConfiguration(ApiKey));
        
        botBotClient.UpdateHandler.Subscribe(
            new BotMessageHandler(new MessageHandler()));
        
        botBotClient.StartReceiving();

        Console.WriteLine("Start receiving");
        Console.ReadKey();
    }
}