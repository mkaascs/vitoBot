using Telegram.Bot.Types;

using Application.Abstractions;
using Application.DTO.Commands;

using Infrastructure.Services.TelegramAPI.Events.Interfaces;
using Infrastructure.Services.TelegramAPI.Application.Extensions;

namespace Infrastructure.Services.TelegramAPI.Application;

public class BotMessageHandler(IMessageHandler messageHandler, IMessageSender messageSender, BotCommandHandler commandHandler) : IAsyncObserver<Message?> {
    public async Task OnNextAsync(Message? message, CancellationToken cancellationToken = default) {
        if (message is null)
            return;

        IEnumerable<SendMessageCommand> answers = (await commandHandler
            .ExecuteCommandIfExists(message, cancellationToken)).ToArray();

        if (answers.Any()) {
            foreach (SendMessageCommand sendMessageCommand in answers)
                await messageSender.SendMessageAsync(sendMessageCommand, cancellationToken);
            
            return;
        }

        answers = await messageHandler.OnGetMessageAsync(message.ToDto(), cancellationToken);
        foreach (SendMessageCommand sendMessageCommand in answers)
            await messageSender.SendMessageAsync(sendMessageCommand, cancellationToken);
    }
    
    public Task OnCompletedAsync(CancellationToken cancellationToken = default) 
        => Task.CompletedTask;

    public Task OnErrorAsync(Exception error, CancellationToken cancellationToken = default)
        => Task.CompletedTask;
}