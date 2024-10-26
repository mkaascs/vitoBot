using Microsoft.EntityFrameworkCore;
using FluentValidation.Results;

using Domain.Abstractions;
using Domain.Entities;
using Domain.VitoAPI;

using Application.Abstractions.BotCommands;
using Application.DTO.Commands;

namespace Application.Services.BotCommands.Settings;

[BotCommand(CommandName = "settings", Description = "Позволяет настроить логику бота")]
public class SettingsBotCommand(
    IUserSettingsRepository userSettingsRepository) : IBotCommand
{
    private readonly SettingsBotCommandValidator _commandValidator = new(3);
    
    public async Task CallAsync(
        IBotCommandHandlingContext context,
        CancellationToken cancellationToken = default)
    {
        UserSettings? userSettings = await userSettingsRepository.Entities
            .Where(userSettings => userSettings.ChatId.Equals(context.Message.Chat.Id))
            .FirstOrDefaultAsync(cancellationToken);
        
        if (context.Arguments.Length == 0)
        {
            await DisplayUserSettingsAsync(
                userSettings,
                context,
                cancellationToken);
            
            return;
        }

        if (context.User.AllowedToChangeUserSettings)
        {
            await UpdateUserSettingsAsync(
                userSettings,
                context, 
                cancellationToken);
            
            return;
        }
        
        await context.AnswerAsync(
            new SendMessageCommand("\u274c У вас недостаточно прав, чтобы менять настройки бота в этом чате", ContentType.Text),
            cancellationToken);
    }

    private async ValueTask UpdateUserSettingsAsync(
        UserSettings? userSettings,
        IBotCommandHandlingContext context,
        CancellationToken cancellationToken = default)
    {
        ValidationResult result = _commandValidator.Validate(context);

        if (result.Errors.Count > 0) {
            await context.AnswerAsync(
                new SendMessageCommand(result.Errors.First().ErrorMessage, ContentType.Text),
                cancellationToken);
            
            return;
        }
        
        if (userSettings is null)
        {
            userSettings = userSettingsRepository.GetDefaultUserSettings();
            userSettings.ChatId = context.Message.Chat.Id;
            userSettingsRepository.Entities.Add(userSettings);
        }
        
        int index = int.Parse(context.Arguments[0]) - 1;
        decimal newChance = decimal.Parse(context.Arguments[1]);

        _mergedChances[index](newChance, userSettings);
        
        await Task.WhenAll(
            userSettingsRepository.SaveChangesAsync(cancellationToken),
            context.AnswerAsync(
                new SendMessageCommand("\u2705 Настройки были успешно изменены", ContentType.Text),
                cancellationToken));
    }

    private async ValueTask DisplayUserSettingsAsync(
        UserSettings? userSettings,
        IBotCommandHandlingContext context, 
        CancellationToken cancellationToken = default)
    {
        userSettings ??= userSettingsRepository.GetDefaultUserSettings();
        
        string content = $"ℹ\ufe0f Действующие настройки VitoBot:\n\n" +
                         $"1) Стандартный шанс отправить сообщение - {userSettings.DefaultChanceToSendMessage:0.######}\n" +
                         $"2) Шанс сохранить текстовое сообщение - {userSettings.ChanceToSaveTextMessage:0.######}\n" +
                         $"3) Шанс сохранить нетекстовое сообщение - {userSettings.ChanceToSaveMessage:0.######}\n\n" +
                         $"Чтобы изменить шанс, передайте аргументы в таком виде:\n[команда] <порядковый номер> <новый шанс>";

        await context.AnswerAsync(new SendMessageCommand(content, ContentType.Text), cancellationToken);
    }
    
    private readonly List<Action<decimal, UserSettings>> _mergedChances = [
        (newChance, userSettings) => userSettings.DefaultChanceToSendMessage = newChance,
        (newChance, userSettings) => userSettings.ChanceToSaveTextMessage = newChance,
        (newChance, userSettings) => userSettings.ChanceToSaveMessage = newChance
    ];
}