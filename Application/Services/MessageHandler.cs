using Application.Abstractions;
using Application.DTO;
using Application.DTO.Commands;
using Application.Services.BotLogic;

namespace Application.Services;

public class MessageHandler(MessageBotSavingLogic messageBotSaver, MessageBotSendingLogic messageBotSender) : IMessageHandler {
    public async Task<IEnumerable<SendMessageCommand>> OnGetMessageAsync(MessageDto message, CancellationToken cancellationToken = default) {
        if (string.IsNullOrWhiteSpace(message.Content))
            return [];

        await messageBotSaver.TryRememberMessageAsync(message, cancellationToken);

        return (await messageBotSender.GetAnswerAsync(message, cancellationToken))
            .Select(answer => new SendMessageCommand(
                message.Chat.Id,
                answer.Content,
                answer.Type));
    }
}