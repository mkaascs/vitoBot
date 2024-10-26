using Application.Abstractions;

using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Infrastructure.Services.TelegramAPI.Application;

public class TelegramUserContext : IUserContext 
{
    public bool AllowedToChangeUserSettings { get; set; }

    public static async Task<TelegramUserContext> RegisterContextFromAsync(
        Message from,
        ITelegramBotClient botClient,
        CancellationToken cancellationToken = default) 
    {
        return from.Chat.Type == ChatType.Private
            ? new TelegramUserContext(true)
            : new TelegramUserContext(from.From, await botClient
                .GetChatAdministratorsAsync(from.Chat.Id, cancellationToken));
    }
    
    private TelegramUserContext(bool isAdmin)
    {
        AllowedToChangeUserSettings = isAdmin;
    }
    
    private TelegramUserContext(
        User? user,
        ChatMember[] admins)
    {
        AllowedToChangeUserSettings = admins.FirstOrDefault(
            admin => admin.User.Id.Equals(user?.Id)) != default;
    }
}