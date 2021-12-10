using Newtonsoft.Json;

namespace DotNET.DDNS.Providers.Ovh.Models
{
    internal class DnsRecord
    {
        [JsonProperty(PropertyName = "target")]
        public string Target { get; set; }

        [JsonProperty(PropertyName = "id")]
        public long Id { get; set; }

        [JsonProperty(PropertyName = "fieldType")]
        public string FieldType { get; set; }

        [JsonProperty(PropertyName = "zone")]
        public string Zone { get; set; }

        [JsonProperty(PropertyName = "subDomain")]
        public string SubDomain { get; set; }

        [JsonProperty(PropertyName = "ttl")]
        public int Ttl { get; set; }
    }
}
