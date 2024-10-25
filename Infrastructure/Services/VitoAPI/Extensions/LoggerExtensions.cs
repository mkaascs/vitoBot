using Infrastructure.Services.VitoAPI.Responses;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services.VitoAPI.Extensions;

internal static class LoggerExtensions
{
    public static void LogResponse(
        this ILogger logger,
        Response response,
        string method)
    {
        string log = $"VitoAPI {method} {response.StatusCode}";
        if (response.IsSuccess)
            logger.LogInformation(log);
        
        else logger.LogError(log + $": {response.Problem?.Title}");
    }
}