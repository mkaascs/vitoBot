using Telegram.Bot;

using Infrastructure.Configuration;
using Infrastructure.Services.TelegramAPI.Events;

namespace Infrastructure.Services.TelegramAPI;

public class BotClient(TelegramApiConfiguration configuration) {
    private readonly TelegramBotClient _botClient = new(configuration.ApiKey);
    
    public TelegramBotUpdateHandler UpdateHandler { get; } = new();

    public async Task<BotInfo> GetBotInfoAsync(CancellationToken cancellationToken = default)
        => new(await _botClient.GetMeAsync(cancellationToken));

    public void StartReceiving()
        => _botClient.StartReceiving(UpdateHandler);
}