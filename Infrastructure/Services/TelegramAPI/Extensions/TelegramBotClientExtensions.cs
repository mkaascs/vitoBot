using Infrastructure.Services.TelegramAPI.Application;

using Telegram.Bot;
using Telegram.Bot.Types;

namespace Infrastructure.Services.TelegramAPI.Extensions;

internal static class TelegramBotClientExtensions {
    public static async Task<TelegramUserContext> GetUserContextFrom(this ITelegramBotClient botClient, Message message,
        CancellationToken cancellationToken = default)
        => await TelegramUserContext.RegisterContextAsync(botClient, message, cancellationToken);
}