using Domain.Entities;

using Microsoft.Extensions.Configuration;

namespace Infrastructure.Configuration;

public class UserSettingsRepositoryConfiguration(IConfiguration configuration) {
    public UserSettings DefaultUserSettings { get; } = configuration
                                                           .GetSection("DefaultUserSettings")
                                                           .Get<UserSettings>()
                                                       ?? throw new InvalidOperationException(
                                                           "There is no default user settings in configuration");
}