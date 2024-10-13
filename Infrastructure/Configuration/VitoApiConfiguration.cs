using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Infrastructure.Configuration;

public class VitoApiConfiguration {
    public VitoApiConfiguration(string domainName) {
        if (string.IsNullOrWhiteSpace(domainName))
            throw new ArgumentException(null, nameof(domainName));

        DomainName = domainName.TrimEnd('/').TrimEnd('\\');
        
        JsonConvert.DefaultSettings = () => new JsonSerializerSettings {
            Converters = { new StringEnumConverter() }
        };
    }
    
    public string DomainName { get; set; }
}