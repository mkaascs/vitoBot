using Domain.Abstractions;
using Domain.Entities;

using Application.DTO;
using Application.DTO.Commands;

namespace Application.Services.BotLogic;

<<<<<<< HEAD
public class MessageBotLogic(MessageSendingLogic sendingLogic, MessageSavingLogic savingLogic, IUserSettingsRepository userSettingsRepository) {
    public async Task<IEnumerable<SendMessageCommand>> OnGetMessageAsync(MessageDto receivedMessage, CancellationToken cancellationToken = default) {
        UserSettings userSettings = await userSettingsRepository
            .GetUserSettingsByChatIdAsync(receivedMessage.Chat.Id, cancellationToken);

        await savingLogic.TryRememberMessageAsync(receivedMessage, userSettings, cancellationToken);

        return (await sendingLogic.GetAnswerAsync(receivedMessage, userSettings, cancellationToken))
            .Select(answer => new SendMessageCommand(receivedMessage.Chat.Id, answer.Content, answer.Type));
=======
public class MessageBotLogic(MessageSavingLogic savingLogic, MessageReplyingLogic replyingLogic, IUserSettingsRepository userSettingsRepository) {
    public async Task<IEnumerable<SendMessageCommand>> GetAnswerAsync(MessageDto receivedMessage, CancellationToken cancellationToken = default) {
        UserSettings currentUserSettings = await userSettingsRepository
            .GetUserSettingsByChatIdAsync(receivedMessage.Chat.Id, cancellationToken);

        Task savingMessage = savingLogic.TryRememberMessageAsync(receivedMessage, currentUserSettings, cancellationToken);
        Task<IEnumerable<SendMessageCommand>> replyingMessage = replyingLogic.GetAnswerAsync(receivedMessage, currentUserSettings, cancellationToken);

        await Task.WhenAll(savingMessage, replyingMessage);

        return await replyingMessage;
>>>>>>> development
    }
}