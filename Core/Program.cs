using Serilog;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Core;

internal class Program {
    private static void Main() {
        IConfiguration configuration = BuildConfiguration();
        
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateLogger();
        
        Log.Logger.Information("Application was started");

        IHost host = BuildHost(configuration);

        host.Services.GetService<App>()?.Run();
    }
    
    private static IConfiguration BuildConfiguration() {
        IConfigurationBuilder builder = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables();
        
        return builder.Build();
    }

    private static IHost BuildHost(IConfiguration configuration) {
        return Host.CreateDefaultBuilder()
            .ConfigureServices(services => {
                Startup.ConfigureServices(configuration, services); })
            .UseSerilog()
            .Build();
    }
}