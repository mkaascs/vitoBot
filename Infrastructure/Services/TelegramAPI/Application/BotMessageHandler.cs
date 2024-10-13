using Application.Abstractions;

using Telegram.Bot.Types;

using Infrastructure.Services.TelegramAPI.Events.Interfaces;
using Infrastructure.Services.TelegramAPI.Application.Extensions;

namespace Infrastructure.Services.TelegramAPI.Application;

public class BotMessageHandler(IMessageHandler messageHandler) : IAsyncObserver<Message?> {
    public async Task OnNextAsync(Message? message) {
        if (message is null)
            return;
        
        await messageHandler.OnGetMessageAsync(message.ToDto());
    }
    
    public Task OnCompletedAsync() 
        => Task.CompletedTask;

    public Task OnErrorAsync(Exception error)
        => Task.CompletedTask;
}