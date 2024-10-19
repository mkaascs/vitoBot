using Application.Abstractions;

using Infrastructure.Services.TelegramAPI;

namespace Core;

public class App(BotClient botClient, IMessageHandler messageHandler) {
    public void Run() {
        botClient.UpdateHandler.Subscribe(messageHandler);
        botClient.StartReceiving();
        
        Console.ReadLine();
    }
}