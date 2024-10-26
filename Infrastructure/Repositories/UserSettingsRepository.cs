using Domain.Abstractions;
using Domain.Entities;
using Infrastructure.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class UserSettingsRepository(
    UserSettingsRepositoryConfiguration configuration,
    ApplicationDbContext dbContext) : IUserSettingsRepository
{
    public DbSet<UserSettings> Entities { get; set; } = dbContext.UserSettings;
    
    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return dbContext.SaveChangesAsync(cancellationToken);
    }

    public UserSettings GetDefaultUserSettings()
    {
        return new UserSettings
        {
            ChatId = 0,
            ChanceToSaveMessage = configuration.DefaultUserSettings.ChanceToSaveMessage,
            ChanceToSaveTextMessage = configuration.DefaultUserSettings.ChanceToSaveTextMessage,
            DefaultChanceToSendMessage = configuration.DefaultUserSettings.DefaultChanceToSendMessage
        };
    }
}