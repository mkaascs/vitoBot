namespace Infrastructure.Configuration;

public class TelegramApiConfiguration {
    public TelegramApiConfiguration(string apiKey) {
        if (string.IsNullOrWhiteSpace(apiKey))
            throw new ArgumentException(null, nameof(apiKey));

        ApiKey = apiKey;
    }
    
    public string ApiKey { get; private set; }
}