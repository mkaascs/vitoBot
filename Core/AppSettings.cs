using Application.Configuration;

using Infrastructure.Configuration;

using Microsoft.Extensions.Configuration;

namespace Core;

public class AppSettings {
    public AppSettings(IConfiguration configuration) {
        IConfigurationSection appSettingsSection = configuration.GetSection("AppSettings");

        BotLogicConfiguration = appSettingsSection.GetSection("BotLogic")
            .Get<BotLogicConfiguration>() ?? throw new InvalidOperationException();

        VitoApiConfiguration = appSettingsSection.GetSection("VitoAPI")
            .Get<VitoApiConfiguration>() ?? throw new InvalidOperationException();

        TelegramApiConfiguration = appSettingsSection.GetSection("TelegramAPI")
            .Get<TelegramApiConfiguration>() ?? throw new InvalidOperationException();
    }
    
    public BotLogicConfiguration BotLogicConfiguration { get; }
    
    public VitoApiConfiguration VitoApiConfiguration { get; }
    
    public TelegramApiConfiguration TelegramApiConfiguration { get; }
}