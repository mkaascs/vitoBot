using Application.Services;
using Application.Services.BotCommands;
using Application.Services.BotLogic;
using Infrastructure.Configuration;
using Infrastructure.Services.TelegramAPI;
using Infrastructure.Services.TelegramAPI.Application;
using Infrastructure.Services.VitoAPI;

using Microsoft.Extensions.Configuration;

namespace Core;

internal class Program {
    private static IConfiguration Configuration { get; } = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", optional: false)
        .Build();

    private static AppSettings Settings { get; } = new(Configuration);
    
    private static void Main() {
        HttpClient httpClient = new();
        BotClient botClient = new(Settings.TelegramApiConfiguration);

        botClient.UpdateHandler.Subscribe(
            new BotMessageHandler(
                new MessageHandler(
                    new BotCommandHandler(
                        '/',
                        new BotMessageSender(botClient),
                        new BotCommandsCollection()),
                    new BotMessageSender(botClient),
                    new MessageSavingLogic(
                        Settings.BotLogicConfiguration,
                        new ChatService(
                            httpClient,
                            Settings.VitoApiConfiguration),
                        new MessageService(
                            httpClient,
                            Settings.VitoApiConfiguration)),
                    new MessageSendingLogic(
                        Settings.BotLogicConfiguration,
                        new MessageService(
                            httpClient,
                            Settings.VitoApiConfiguration)))));
        
        botClient.StartReceiving();
        Console.ReadKey();
    }
}