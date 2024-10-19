using Telegram.Bot;

using Telegram.Bot.Polling;

using Infrastructure.Configuration;

namespace Infrastructure.Services.TelegramAPI;

public class BotClient(TelegramApiConfiguration configuration, TelegramBotUpdateHandler updateHandler) {
    public TelegramBotClient Bot { get; } = new(configuration.ApiKey);
    
    public TelegramBotUpdateHandler UpdateHandler { get; } = updateHandler;
    
    public async Task<BotInfo> GetBotInfoAsync(CancellationToken cancellationToken = default)
        => new(await Bot.GetMeAsync(cancellationToken));

    public void StartReceiving() {
        Bot.StartReceiving(UpdateHandler,
            new ReceiverOptions {
                ThrowPendingUpdates = true });
    }
}