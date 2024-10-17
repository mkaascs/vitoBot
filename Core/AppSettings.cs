using Domain.Entities;

using Infrastructure.Configuration;

using Microsoft.Extensions.Configuration;

namespace Core;

public class AppSettings {
    public AppSettings(IConfiguration configuration) {
        IConfigurationSection appSettingsSection = configuration.GetSection("AppSettings");

        DefaultUserSettings = appSettingsSection.GetSection("DefaultSettings")
            .Get<UserSettings>() ?? throw new InvalidOperationException();

        VitoApiConfiguration = appSettingsSection.GetSection("VitoAPI")
            .Get<VitoApiConfiguration>() ?? throw new InvalidOperationException();

        TelegramApiConfiguration = appSettingsSection.GetSection("TelegramAPI")
            .Get<TelegramApiConfiguration>() ?? throw new InvalidOperationException();
    }
    
    public UserSettings DefaultUserSettings { get; }
    
    public VitoApiConfiguration VitoApiConfiguration { get; }
    
    public TelegramApiConfiguration TelegramApiConfiguration { get; }
}