using Application.Services;
using Application.Services.BotCommands;
using Application.Services.BotLogic;
using Infrastructure;
using Infrastructure.Configuration;
using Infrastructure.Repositories;
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
        using HttpClient httpClient = new();
        using ApplicationDbContext dbContext = new();
        
        BotClient botClient = new(Settings.TelegramApiConfiguration);
        UserSettingsRepository userSettingsRepository = new(dbContext, Settings.DefaultUserSettings);

        botClient.UpdateHandler.Subscribe(
            new BotMessageHandler(
                new MessageHandler(
                    new BotCommandHandler(
                        '/',
                        new BotMessageSender(botClient),
                        new BotCommandsCollection(userSettingsRepository)),
                    new BotMessageSender(botClient),
                    new MessageSavingLogic(
                        new ChatService(httpClient, Settings.VitoApiConfiguration),
                        new MessageService(httpClient, Settings.VitoApiConfiguration)),
                    new MessageSendingLogic(
                        new MessageService(httpClient, Settings.VitoApiConfiguration)),
                    userSettingsRepository)));
        
        botClient.StartReceiving();
        Console.ReadKey();
    }
}