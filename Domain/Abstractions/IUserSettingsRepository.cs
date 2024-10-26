using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Domain.Abstractions;

public interface IUserSettingsRepository
{
    public DbSet<UserSettings> Entities { get; set; }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default);

    public UserSettings GetDefaultUserSettings();
}