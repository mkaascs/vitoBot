using Domain.Abstractions;
using Domain.Entities;

using Application.DTO;
using Application.DTO.Commands;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.BotLogic;

public class MessageBotLogic(
    MessageSavingLogic savingLogic, 
    MessageReplyingLogic replyingLogic,
    IUserSettingsRepository userSettingsRepository)
{
    public async Task<IEnumerable<SendMessageCommand>> GetReplyAsync(
        MessageDto receivedMessage,
        CancellationToken cancellationToken = default)
    {
        UserSettings userSettingsInChat = await userSettingsRepository.Entities
            .Where(userSettings => userSettings.ChatId.Equals(receivedMessage.Chat.Id))
            .FirstOrDefaultAsync(cancellationToken)
            ?? userSettingsRepository.GetDefaultUserSettings();

        Task savingMessage = savingLogic
            .TryRememberMessageAsync(receivedMessage, userSettingsInChat, cancellationToken);
        
        Task<IEnumerable<SendMessageCommand>> replyingMessage = replyingLogic
            .GetAnswerAsync(receivedMessage, userSettingsInChat, cancellationToken);

        await Task.WhenAll(savingMessage, replyingMessage);

        return await replyingMessage;
    }
}