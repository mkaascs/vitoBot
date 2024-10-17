using Domain.VitoAPI;

using Application.Abstractions;
using Application.DTO;
using Application.DTO.Commands;
using Application.Services.BotCommands;
using Application.Services.BotLogic;

namespace Application.Services;

public class MessageHandler(BotCommandHandler commandHandler, IMessageSender messageSender, MessageSavingLogic messageSavingLogic, MessageSendingLogic messageSendingLogic) : IMessageHandler {
    public async Task OnGetMessageAsync(MessageDto message, CancellationToken cancellationToken = default) {
        if (string.IsNullOrWhiteSpace(message.Content))
            return;

        if (await commandHandler.TryExecuteCommand(message, cancellationToken)) 
            return;

        await messageSavingLogic.TryRememberMessageAsync(message, cancellationToken);

        IEnumerable<Message> answers = await messageSendingLogic.GetAnswerAsync(message, cancellationToken);
        foreach (Message answer in answers)
            await messageSender.SendMessageAsync(
                new SendMessageCommand(message.Chat.Id, answer.Content, answer.Type),
                cancellationToken);
    }
}