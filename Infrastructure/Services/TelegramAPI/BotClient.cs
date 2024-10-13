using Telegram.Bot;

using Infrastructure.Configuration;
using Infrastructure.Services.TelegramAPI.Events;

namespace Infrastructure.Services.TelegramAPI;

public class BotClient(TelegramApiConfiguration configuration) {
    public TelegramBotClient Bot { get; } = new(configuration.ApiKey);
    
    public TelegramBotUpdateHandler UpdateHandler { get; } = new();
    
    public async Task<BotInfo> GetBotInfoAsync(CancellationToken cancellationToken = default)
        => new(await Bot.GetMeAsync(cancellationToken));
    
    public void StartReceiving()
        => Bot.StartReceiving(UpdateHandler);
}