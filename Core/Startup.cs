using Core.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Core;

internal sealed class Startup {
    public static IServiceCollection ConfigureServices(IConfiguration configuration, IServiceCollection services) {
        services.AddTransient<App>();
        services.AddSingleton(configuration);
        services.AddSingleton<IServiceProvider>(services.BuildServiceProvider());

        services.AddVitoApiServices();
        
        services.AddTelegramApiServices();
        
        services.AddRepositories(configuration);
        
        services.AddMessageHandlers();
        
        return services;
    }
}