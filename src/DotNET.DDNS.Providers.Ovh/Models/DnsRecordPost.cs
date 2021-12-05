using Newtonsoft.Json;

namespace DotNET.DDNS.Providers.Ovh.Models
{
    internal class DnsRecordPost
    {
        [JsonProperty(PropertyName = "target")]
        public string Target { get; set; }

        [JsonProperty(PropertyName = "fieldType")]
        public string FieldType { get; set; }

        [JsonProperty(PropertyName = "subDomain")]
        public string SubDomain { get; set; }

        [JsonProperty(PropertyName = "ttl")]
        public int Ttl { get; set; }

        public DnsRecordPost(string target, string fieldType, string subDomain, int ttl)
        {
            Target = target;
            FieldType = fieldType;
            SubDomain = subDomain;
            Ttl = ttl;
        }
    }
}
