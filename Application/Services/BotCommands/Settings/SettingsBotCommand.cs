using FluentValidation;
using FluentValidation.Results;

using Domain.Abstractions;
using Domain.Entities;
using Domain.VitoAPI;

using Application.Abstractions.BotCommands;
using Application.DTO.Commands;

namespace Application.Services.BotCommands.Settings;

[BotCommand(CommandName = "settings", Description = "Позволяет настроить логику бота")]
public class SettingsBotCommand : IBotCommand {
    private readonly List<Action<decimal, UserSettings>> _mergedChances = [
        (newChance, userSettings) => userSettings.DefaultChanceToSendMessage = newChance,
        (newChance, userSettings) => userSettings.ChanceToSaveTextMessage = newChance,
        (newChance, userSettings) => userSettings.ChanceToSaveMessage = newChance
    ];

    private readonly IUserSettingsRepository _settingsRepository;
    private readonly IValidator<IBotCommandHandlingContext> _commandValidator;

    public SettingsBotCommand(IUserSettingsRepository settingsRepository) {
        ArgumentNullException.ThrowIfNull(settingsRepository);
        _settingsRepository = settingsRepository;
        _commandValidator = new SettingsBotCommandValidator(_mergedChances.Count);
    }
    
    public async Task CallAsync(IBotCommandHandlingContext context, CancellationToken cancellationToken = default) {
        UserSettings userSettings = await _settingsRepository
            .GetUserSettingsByChatIdAsync(context.Message.Chat.Id, cancellationToken);
        
        if (context.Arguments.Length == 0) {
            await DisplayUserSettingsAsync(context, userSettings, cancellationToken);
            return;
        }

        ValidationResult result = await _commandValidator.ValidateAsync(context, cancellationToken);

        if (result.Errors.Count > 0) {
            string errorMessage = result.Errors.First().ErrorMessage;
            await context.AnswerAsync(new SendMessageCommand(errorMessage, ContentType.Text), cancellationToken);
            return;
        }

        int index = int.Parse(context.Arguments[0]) - 1;
        decimal newChance = decimal.Parse(context.Arguments[1]);

        _mergedChances[index](newChance, userSettings);
        await _settingsRepository.UpdateUserSettingsAsync(userSettings, cancellationToken);
        await context.AnswerAsync(
            new SendMessageCommand("\u2705 Настройки были успешно изменены", ContentType.Text),
            cancellationToken);
    }

    private async Task DisplayUserSettingsAsync(IBotCommandHandlingContext context, UserSettings userSettings, CancellationToken cancellationToken = default) {
        string content = $"ℹ\ufe0f Действующие настройки VitoBot:\n\n" +
                         $"1) Стандартный шанс отправить сообщение - {userSettings.DefaultChanceToSendMessage:0.######}\n" +
                         $"2) Шанс сохранить текстовое сообщение - {userSettings.ChanceToSaveTextMessage:0.######}\n" +
                         $"3) Шанс сохранить нетекстовое сообщение - {userSettings.ChanceToSaveMessage:0.######}\n\n" +
                         $"Чтобы изменить шанс, передайте аргументы в таком виде:\n[команда] <порядковый номер> <новый шанс>";

        await context.AnswerAsync(new SendMessageCommand(content, ContentType.Text), cancellationToken);
    }
}