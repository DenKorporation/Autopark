namespace Autopark.Common.Configuration;

public class IdentityConfiguration
{
    public string Realm { get; set; }
    public string BaseUrl { get; set; }
    public string OIDCEndpoint { get; set; }
    public string TokenEndpoint { get; set; }
    public string AuthorizationEndpoint { get; set; }
    public string Scope { get; set; }
    public string ClientId { get; set; }
    public string Audience { get; set; }
    public AdminConfiguration AdminConfiguration { get; set; }
}
