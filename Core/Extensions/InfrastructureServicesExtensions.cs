using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Domain.Abstractions;

using Application.Abstractions;

using Infrastructure;
using Infrastructure.Configuration;
using Infrastructure.Repositories;
using Infrastructure.Services.TelegramAPI;
using Infrastructure.Services.TelegramAPI.Application;
using Infrastructure.Services.VitoAPI;

namespace Core.Extensions;

internal static class InfrastructureServicesExtensions {
    public static IServiceCollection AddVitoApiServices(this IServiceCollection services) {
        services.AddSingleton<HttpClient>();
        services.AddSingleton<VitoApiConfiguration>();
        
        services.AddTransient<IMessageApiService, MessageService>();
        services.AddTransient<IChatApiService, ChatService>();

        return services;
    }

    public static IServiceCollection AddTelegramApiServices(this IServiceCollection services) {
        services.AddSingleton<BotClient>();
        services.AddSingleton<TelegramApiConfiguration>();
        
        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configuration) {
        string connectionString = configuration["ConnectionStrings:UserSettingsDb"]
            ?? throw new InvalidOperationException("There is no user settings db connection string in configuration");

        services.AddDbContext<ApplicationDbContext>(options
            => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

        services.AddSingleton<ApplicationDbContext>();
        services.AddSingleton<UserSettingsRepositoryConfiguration>();
        services.AddSingleton<IUserSettingsRepository, UserSettingsRepository>();
        
        return services;
    }
}