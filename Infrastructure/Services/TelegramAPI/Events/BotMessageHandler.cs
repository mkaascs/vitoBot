using Application.Abstractions;

using Telegram.Bot.Types;

using Infrastructure.Services.TelegramAPI.Extensions;

namespace Infrastructure.Services.TelegramAPI.Events;

public class BotMessageHandler(IMessageHandler messageHandler) : IObserver<Message?> {
    public void OnNext(Message? message) {
        if (message is null)
            return;
        
        messageHandler.OnGetMessage(message.ToDto());
    }
    
    public void OnCompleted() { }

    public void OnError(Exception error) { }
}