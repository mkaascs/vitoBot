using Application.Services;

using Infrastructure.Configuration;
using Infrastructure.Services.TelegramAPI;
using Infrastructure.Services.TelegramAPI.Application;

namespace Core;

internal class Program {
    private const string ApiKey = "5941206869:AAFKW9x9hNOVh5I9ez-XMgJ8YK0KYn_0GPk";
    private static void Main(string[] args) {
        BotClient botClient = new BotClient(new TelegramApiConfiguration(ApiKey));
        
        botClient.UpdateHandler.Subscribe(
            new BotMessageHandler(
                new MessageHandler(
                    new BotMessageSender(botClient))));
        
        botClient.StartReceiving();

        Console.WriteLine("Start receiving");
        Console.ReadKey();
    }
}