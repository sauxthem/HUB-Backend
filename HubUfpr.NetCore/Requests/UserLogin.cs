using Newtonsoft.Json;

namespace HubUfpr.API.Requests
{
    public class UserLogin
    {
        [JsonProperty(PropertyName = "usuario", NullValueHandling = NullValueHandling.Ignore)]
        public string usuario { get; set; }

        [JsonProperty(PropertyName = "senha", NullValueHandling = NullValueHandling.Ignore)]
        public string senha { get; set; }
    }
}