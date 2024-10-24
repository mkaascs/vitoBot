using Microsoft.EntityFrameworkCore;

using Domain.Abstractions;
using Domain.Entities;

using Infrastructure.Configuration;

namespace Infrastructure.Repositories;

public class UserSettingsRepository(ApplicationDbContext dbContext, UserSettingsRepositoryConfiguration configuration) : IUserSettingsRepository {
    public async Task<UserSettings> GetUserSettingsByChatIdAsync(ulong chatId, CancellationToken cancellationToken = default) {
        return await dbContext.UserSettings
            .SingleOrDefaultAsync(userSettings => userSettings.ChatId.Equals(chatId), cancellationToken)
            ?? configuration.DefaultUserSettings;
    }

    public async Task UpdateUserSettingsAsync(UserSettings userSettings, CancellationToken cancellationToken = default) {
        UserSettings? foundUserSettings = await dbContext.UserSettings
            .SingleOrDefaultAsync(settings => settings.ChatId.Equals(userSettings.ChatId), cancellationToken);

        if (foundUserSettings is null) {
            dbContext.Add(userSettings);
            await dbContext.SaveChangesAsync(cancellationToken);
            return;
        }
        
        UpdateUserSettings(foundUserSettings, userSettings);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private static void UpdateUserSettings(UserSettings oldSettings, UserSettings newSettings) {
        oldSettings.ChanceToSaveMessage = newSettings.ChanceToSaveMessage;
        oldSettings.ChanceToSaveTextMessage = newSettings.ChanceToSaveTextMessage;
        oldSettings.DefaultChanceToSendMessage = newSettings.DefaultChanceToSendMessage;
    }
}