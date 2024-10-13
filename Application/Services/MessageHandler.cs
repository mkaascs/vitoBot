using Domain.VitoAPI;

using Application.Abstractions;
using Application.DTO;
using Application.DTO.Commands;

namespace Application.Services;

public class MessageHandler(MessageBotSavingLogic messageBotSaver, MessageBotSendingLogic messageBotSender, IMessageSender messageSender) : IMessageHandler {
    public async Task OnGetMessageAsync(MessageDto message, CancellationToken cancellationToken = default) {
        if (string.IsNullOrWhiteSpace(message.Content))
            return;

        await messageBotSaver.TryRememberMessageAsync(message, cancellationToken);

        IEnumerable<Message> answers = await messageBotSender.GetAnswerAsync(message, cancellationToken);
        foreach (Message answer in answers)
            await messageSender.SendMessageAsync(
                new SendMessageCommand(
                    message.Chat.Id,
                    answer.Content,
                    answer.Type),
                cancellationToken);
    }
}