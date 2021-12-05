namespace DotNET.DDNS.Providers.Models
{
    public class ProviderOptionsModel
    {
        public string? Provider { get; set; }

        public string? Domain { get; set; }

        public string? SubDomain { get; set; }

        public string? Secret { get; set; }
    }
}
