using Domain.Entities;

namespace Domain.Abstractions;

/// <summary>
/// An interface for managing an <see cref="UserSettings"/> repository
/// </summary>
public interface IUserSettingsRepository {
    /// <summary>
    /// An asynchronous method to get a user settings by chat id
    /// </summary>
    /// <param name="chatId">Unique id of chat</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Returns completed task with a found <see cref="UserSettings"/> instance</returns>
    Task<UserSettings> GetUserSettingsByChatIdAsync(ulong chatId, CancellationToken cancellationToken = default);

    /// <summary>
    /// An asynchronous method to update the user settings 
    /// </summary>
    /// <param name="userSettings">New instance of <see cref="UserSettings"/></param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Returns completed task</returns>
    Task UpdateUserSettingsAsync(UserSettings userSettings, CancellationToken cancellationToken = default);
}