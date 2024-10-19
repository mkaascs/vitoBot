using Microsoft.Extensions.Configuration;

namespace Infrastructure.Configuration;

public class TelegramApiConfiguration(IConfiguration configuration) {
    public string ApiKey { get; } = configuration["TelegramAPI:ApiKey"] 
                                    ?? throw new InvalidOperationException("There is no api key in TelegramApi configuration");
}