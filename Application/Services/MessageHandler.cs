using Microsoft.Extensions.Logging;

using Domain.Abstractions;
using Domain.VitoAPI;
using Domain.Entities;

using Application.Abstractions;
using Application.DTO;
using Application.DTO.Commands;
using Application.Services.BotCommands;
using Application.Services.BotLogic;

namespace Application.Services;

public class MessageHandler(BotCommandHandler commandHandler, IMessageSender messageSender, MessageSavingLogic messageSavingLogic, MessageSendingLogic messageSendingLogic, IUserSettingsRepository settingsRepository, ILogger<MessageHandler> logger) : IMessageHandler {
    public async Task OnGetMessageAsync(MessageDto message, CancellationToken cancellationToken = default) {
        if (string.IsNullOrWhiteSpace(message.Content))
            return;

        if (await commandHandler.TryExecuteCommand(message, cancellationToken)) 
            return;

        UserSettings userSettings = await settingsRepository
            .GetUserSettingsByChatIdAsync(message.Chat.Id, cancellationToken);        

        await messageSavingLogic.TryRememberMessageAsync(message, userSettings, cancellationToken);

        Message[] answers = (await messageSendingLogic.GetAnswerAsync(message, userSettings, cancellationToken)).ToArray();
        foreach (Message answer in answers)
            await messageSender.SendMessageAsync(
                new SendMessageCommand(message.Chat.Id, answer.Content, answer.Type),
                cancellationToken);
        
        logger.LogInformation("{count} messages were sent to {chat}", answers.Length, message.From?.FirstName);
    }
}