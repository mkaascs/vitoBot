using Application.Configuration;
using Application.Services;

using Infrastructure.Configuration;
using Infrastructure.Services.TelegramAPI;
using Infrastructure.Services.TelegramAPI.Application;
using Infrastructure.Services.VitoAPI;

namespace Core;

internal class Program {
    #region Configuration

    private static readonly TelegramApiConfiguration TelegramApiConfiguration = new("5941206869:AAFKW9x9hNOVh5I9ez-XMgJ8YK0KYn_0GPk");
    
    private static readonly VitoApiConfiguration VitoApiConfiguration = new("http://localhost:5000");
    
    private static readonly BotLogicConfiguration LogicConfiguration = new() {
        ChanceToSaveMessage    = 0.75,
        ChanceToSaveTextMessage = 0.6,
        DefaultChanceToSendMessage = 0.45
    };

    #endregion
    private static void Main(string[] args) {
        BotClient botClient = new BotClient(TelegramApiConfiguration);
        
        HttpClient httpClient = new();

        botClient.UpdateHandler.Subscribe(
            new BotMessageHandler(
                new MessageHandler(
                    new MessageBotSavingLogic(
                        LogicConfiguration,
                        new ChatService(httpClient, VitoApiConfiguration),
                        new MessageService(httpClient, VitoApiConfiguration)),
                    new MessageBotSendingLogic(
                        LogicConfiguration,
                        new MessageService(httpClient, VitoApiConfiguration)),
                    new BotMessageSender(botClient))));
        
        botClient.StartReceiving();
        Console.ReadKey();
    }
}