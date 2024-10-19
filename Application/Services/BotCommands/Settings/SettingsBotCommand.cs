using Domain.Abstractions;
using Domain.Entities;
using Domain.VitoAPI;

using Application.Abstractions.BotCommands;
using Application.DTO;
using Application.DTO.Commands;

namespace Application.Services.BotCommands.Settings;

[BotCommand(CommandName = "settings", Description = "Allows you to change the chances of sending and saving messages")]
public class SettingsBotCommand(IUserSettingsRepository settingsRepository) : IBotCommand {
    private readonly List<Action<decimal, UserSettings>> _chances = [
        (newChance, userSettings) => userSettings.DefaultChanceToSendMessage = newChance,
        (newChance, userSettings) => userSettings.ChanceToSaveTextMessage = newChance,
        (newChance, userSettings) => userSettings.ChanceToSaveMessage = newChance
    ];
    
    public async Task<IEnumerable<SendMessageCommand>> CallAsync(MessageDto callingMessage, string[] arguments,
        CancellationToken cancellationToken = default) {
        
        if (arguments.Length == 0)
            return await CallAsync(callingMessage, cancellationToken);

        UserSettings userSettings = await settingsRepository
            .GetUserSettingsByChatIdAsync(callingMessage.Chat.Id, cancellationToken); 

        if (!int.TryParse(arguments[0], out int index) || !decimal.TryParse(arguments[1], out decimal newChance))
            return new[] {
                new SendMessageCommand(callingMessage.Chat.Id, "\u274c Incorrect arguments format", ContentType.Text)
            };

        if (index is < 1 or > 3)
            return new[] {
                new SendMessageCommand(callingMessage.Chat.Id, "\u274c Incorrect index. It must be no less than 1 and no more than 3", ContentType.Text)
            };
        
        if (newChance is < 0m or > 1m)
            return new[] {
                new SendMessageCommand(callingMessage.Chat.Id, "\u274c Incorrect new chance. It must be no less than 0 and no more than 1", ContentType.Text)
            };

        _chances[index - 1](newChance, userSettings);
        userSettings.ChatId = callingMessage.Chat.Id;
        
        await settingsRepository.UpdateUserSettingsAsync(userSettings, cancellationToken);

        return new[] {
            new SendMessageCommand(callingMessage.Chat.Id, "\u2705 Settings was changed successfully", ContentType.Text)
        };
    }

    private async Task<IEnumerable<SendMessageCommand>> CallAsync(MessageDto callingMessage,
        CancellationToken cancellationToken = default) {

        UserSettings userSettings = await settingsRepository
            .GetUserSettingsByChatIdAsync(callingMessage.Chat.Id, cancellationToken);

        string content = $"ℹ\ufe0f Current vitoBot settings:\n\n" +
                         $"1) A default chance to send message - {userSettings.DefaultChanceToSendMessage:0.######}\n" +
                         $"2) A chance to save text message - {userSettings.ChanceToSaveTextMessage:0.######}\n" +
                         $"3) A chance to save non-text message - {userSettings.ChanceToSaveMessage:0.######}\n\n" +
                         $"ℹ\ufe0f To change the chance pass the arguments like this:\n[command] <index> <newChance>";
        
        return new[] {
            new SendMessageCommand(callingMessage.Chat.Id, content, ContentType.Text)
        };
    }
}