using Microsoft.Extensions.Logging;

using Telegram.Bot;
using Telegram.Bot.Polling;

using Infrastructure.Configuration;
using Infrastructure.Services.TelegramAPI.Application;

namespace Infrastructure.Services.TelegramAPI;

public class BotClient
{
    private readonly TelegramBotClient _botClient;

    public BotClient(TelegramApiConfiguration configuration, ILogger<BotClient> logger) 
    {
        ArgumentNullException.ThrowIfNull(configuration);

        _botClient = new TelegramBotClient(configuration.ApiKey);
        TelegramBotMessageSender messageSender = new(_botClient, logger);
        
        UpdateHandler = new TelegramBotUpdateHandler(messageSender, logger);
    }
    
    public TelegramBotUpdateHandler UpdateHandler { get; }
    
    public void StartReceiving() 
    {
        _botClient.StartReceiving(UpdateHandler,
            new ReceiverOptions {
                ThrowPendingUpdates = true });
    }
}