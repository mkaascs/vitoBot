using Application.Abstractions;

using Infrastructure.Services.TelegramAPI;
using Microsoft.Extensions.Logging;
using Serilog.Core;

namespace Core;

public class App(BotClient botClient, IEnumerable<IMessageHandler> messageHandlers) {
    public void Run() {
        foreach (IMessageHandler messageHandler in messageHandlers)
            botClient.UpdateHandler.Subscribe(messageHandler);
        
        botClient.StartReceiving();
        Console.ReadLine();
    }
}