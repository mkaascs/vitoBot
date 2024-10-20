using Domain.Abstractions;
using Domain.Entities;
using Domain.VitoAPI;

using Application.Abstractions.BotCommands;
using Application.DTO;
using Application.DTO.Commands;

namespace Application.Services.BotCommands.Settings;

[BotCommand(CommandName = "settings", Description = "Позволяет настроить логику бота")]
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
                new SendMessageCommand(callingMessage.Chat.Id, "\u274c Неверный формат аргументов", ContentType.Text)
            };

        if (index is < 1 or > 3)
            return new[] {
                new SendMessageCommand(callingMessage.Chat.Id, "\u274c Неверный индекс. Он не должен быть меньше 1 и больше 3", ContentType.Text)
            };
        
        if (newChance is < 0m or > 1m)
            return new[] {
                new SendMessageCommand(callingMessage.Chat.Id, "\u274c Неверный шанс. Он должен быть дробным числом от 0 до 1 включительно", ContentType.Text)
            };

        _chances[index - 1](newChance, userSettings);
        userSettings.ChatId = callingMessage.Chat.Id;
        
        await settingsRepository.UpdateUserSettingsAsync(userSettings, cancellationToken);

        return new[] {
            new SendMessageCommand(callingMessage.Chat.Id, "\u2705 Настройки были изменены успешно", ContentType.Text)
        };
    }

    private async Task<IEnumerable<SendMessageCommand>> CallAsync(MessageDto callingMessage,
        CancellationToken cancellationToken = default) {

        UserSettings userSettings = await settingsRepository
            .GetUserSettingsByChatIdAsync(callingMessage.Chat.Id, cancellationToken);

        string content = $"ℹ\ufe0f Действующие настройки VitoBot:\n\n" +
                         $"1) Стандартный шанс отправить сообщение - {userSettings.DefaultChanceToSendMessage:0.######}\n" +
                         $"2) Шанс сохранить текстовое сообщение - {userSettings.ChanceToSaveTextMessage:0.######}\n" +
                         $"3) Шанс сохранить нетекстовое сообщение - {userSettings.ChanceToSaveMessage:0.######}\n\n" +
                         $"Чтобы изменить шанс, передайте аргументы в таком виде:\n[команда] <порядковый номер> <новый шанс>";
        
        return new[] {
            new SendMessageCommand(callingMessage.Chat.Id, content, ContentType.Text)
        };
    }
}