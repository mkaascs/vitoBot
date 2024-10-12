namespace Infrastructure.Configuration;

public class VitoApiConfiguration {
    public VitoApiConfiguration(string domainName) {
        if (string.IsNullOrWhiteSpace(domainName))
            throw new ArgumentException(null, nameof(domainName));

        DomainName = domainName.TrimEnd('/').TrimEnd('\\');
    }
    
    public string DomainName { get; private set; }
}