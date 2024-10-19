using Microsoft.Extensions.Configuration;

namespace Infrastructure.Configuration;

public class VitoApiConfiguration(IConfiguration configuration) {
    public string DomainName { get; } = configuration["VitoAPI:DomainName"]
                                        ?? throw new InvalidOperationException("There is no domain name in VitoApi configuration");
}