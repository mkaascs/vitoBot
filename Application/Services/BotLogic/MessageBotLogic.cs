using Domain.Abstractions;
using Domain.Entities;

using Application.DTO;
using Application.DTO.Commands;

namespace Application.Services.BotLogic;

public class MessageBotLogic(MessageSendingLogic sendingLogic, MessageSavingLogic savingLogic, IUserSettingsRepository userSettingsRepository) {
    public async Task<IEnumerable<SendMessageCommand>> OnGetMessageAsync(MessageDto receivedMessage, CancellationToken cancellationToken = default) {
        UserSettings userSettings = await userSettingsRepository
            .GetUserSettingsByChatIdAsync(receivedMessage.Chat.Id, cancellationToken);

        await savingLogic.TryRememberMessageAsync(receivedMessage, userSettings, cancellationToken);

        return (await sendingLogic.GetAnswerAsync(receivedMessage, userSettings, cancellationToken))
            .Select(answer => new SendMessageCommand(receivedMessage.Chat.Id, answer.Content, answer.Type));
    }
}