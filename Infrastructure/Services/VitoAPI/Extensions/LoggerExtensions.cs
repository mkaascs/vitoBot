using Domain.VitoAPI;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services.VitoAPI.Extensions;

internal static class LoggerExtensions
{
    public static void LogResponse(
        this ILogger logger,
        Response response,
        string method,
        string description)
    {
        
        string log = $"{method} {response.StatusCode}: VitoAPI {description}";
        if (response.IsSuccess)
            logger.LogInformation(log);
        
        else logger.LogError(log);
    }
}