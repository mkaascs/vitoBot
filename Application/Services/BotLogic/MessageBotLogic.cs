using Domain.Abstractions;
using Domain.Entities;

using Application.DTO;
using Application.DTO.Commands;

namespace Application.Services.BotLogic;

public class MessageBotLogic(MessageSavingLogic savingLogic, MessageReplyingLogic replyingLogic, IUserSettingsRepository userSettingsRepository) {
    public async Task<IEnumerable<SendMessageCommand>> GetAnswerAsync(MessageDto receivedMessage, CancellationToken cancellationToken = default) {
        UserSettings currentUserSettings = await userSettingsRepository
            .GetUserSettingsByChatIdAsync(receivedMessage.Chat.Id, cancellationToken);

        Task savingMessage = savingLogic.TryRememberMessageAsync(receivedMessage, currentUserSettings, cancellationToken);
        Task<IEnumerable<SendMessageCommand>> replyingMessage = replyingLogic.GetAnswerAsync(receivedMessage, currentUserSettings, cancellationToken);

        await Task.WhenAll(savingMessage, replyingMessage);

        return await replyingMessage;
    }
}