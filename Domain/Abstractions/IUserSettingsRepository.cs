using Domain.Entities;

namespace Domain.Abstractions;

public interface IUserSettingsRepository {
    Task<UserSettings> GetUserSettingsByChatIdAsync(ulong chatId, CancellationToken cancellationToken = default);

    Task UpdateUserSettingsAsync(UserSettings userSettings, CancellationToken cancellationToken = default);
}