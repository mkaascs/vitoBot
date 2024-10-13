using Application.Services;

using Infrastructure.Services.TelegramAPI;
using Infrastructure.Services.TelegramAPI.Application;
using Infrastructure.Services.VitoAPI;

using Microsoft.Extensions.Configuration;

namespace Core;

internal class Program {
    private static IConfiguration Configuration { get; } = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false)
        .Build();

    private static AppSettings Settings { get; } = new(Configuration);
    
    private static void Main() {
        HttpClient httpClient = new();
        BotClient botClient = new(Settings.TelegramApiConfiguration);
        
        botClient.UpdateHandler.Subscribe(
            new BotMessageHandler(
                new MessageHandler(
                    new MessageBotSavingLogic(
                        Settings.BotLogicConfiguration,
                        new ChatService(httpClient, Settings.VitoApiConfiguration),
                        new MessageService(httpClient, Settings.VitoApiConfiguration)),
                    new MessageBotSendingLogic(
                        Settings.BotLogicConfiguration,
                        new MessageService(httpClient, Settings.VitoApiConfiguration)),
                    new BotMessageSender(botClient))));
        
        botClient.StartReceiving();
        Console.ReadKey();
    }
}