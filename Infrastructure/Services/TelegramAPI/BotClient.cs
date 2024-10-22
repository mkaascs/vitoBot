using Telegram.Bot;
using Telegram.Bot.Polling;

using Infrastructure.Configuration;
using Infrastructure.Services.TelegramAPI.Application;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services.TelegramAPI;

public class BotClient {
    public BotClient(TelegramApiConfiguration configuration, ILogger<BotClient> logger) {
        ArgumentNullException.ThrowIfNull(configuration);

        Bot = new TelegramBotClient(configuration.ApiKey);
        MessageSender = new TelegramBotMessageSender(this, logger);
        UpdateHandler = new TelegramBotUpdateHandler(MessageSender, logger);
    }
    
    public TelegramBotClient Bot { get; }
    
    public TelegramBotUpdateHandler UpdateHandler { get; }

    public TelegramBotMessageSender MessageSender { get; }
    
    public void StartReceiving() {
        Bot.StartReceiving(UpdateHandler,
            new ReceiverOptions {
                ThrowPendingUpdates = true });
    }
}