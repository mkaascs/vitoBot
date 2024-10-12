using Telegram.Bot.Types;

namespace Infrastructure.Services.TelegramAPI;

public class BotInfo(User bot) {
    public long Id => bot.Id;

    public string? Username => bot.Username;

    public string? FirstName => bot.FirstName;
}