using Microsoft.Extensions.DependencyInjection;

using Application.Abstractions;
using Application.Services;
using Application.Services.BotCommands;
using Application.Services.BotLogic;

namespace Core.Extensions;

internal static class ApplicationServiceExtensions {
    public static IServiceCollection AddMessageHandlers(this IServiceCollection services) {
        services.AddSingleton<BotCommandsCollection>();
        
        services.AddTransient<IMessageHandler, MessageHandler>();
        services.AddTransient<IMessageHandler, BotCommandHandler>();
        services.AddTransient<MessageReplyingLogic>();
        services.AddTransient<MessageSavingLogic>();
        services.AddTransient<MessageBotLogic>();
        
        return services;
    }
}