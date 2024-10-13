using Application.Abstractions;

using Telegram.Bot.Types;

using Infrastructure.Services.TelegramAPI.Events.Interfaces;
using Infrastructure.Services.TelegramAPI.Application.Extensions;

namespace Infrastructure.Services.TelegramAPI.Application;

public class BotMessageHandler(IMessageHandler messageHandler) : IAsyncObserver<Message?> {
    public async Task OnNextAsync(Message? message, CancellationToken cancellationToken = default) {
        if (message is null)
            return;
        
        await messageHandler.OnGetMessageAsync(message.ToDto(), cancellationToken);
    }
    
    public Task OnCompletedAsync(CancellationToken cancellationToken = default) 
        => Task.CompletedTask;

    public Task OnErrorAsync(Exception error, CancellationToken cancellationToken = default)
        => Task.CompletedTask;
}