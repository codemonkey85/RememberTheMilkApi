using Newtonsoft.Json;

namespace RememberTheMilkApi.Objects
{
    [JsonObject("auth")]
    public class RtmApiAuthResponse
    {
        [JsonProperty("token")]
        public string token { get; set; }

        [JsonProperty("perms")]
        public string perms { get; set; }

        [JsonProperty("user")]
        public RtmApiUser user { get; set; }

        public RtmApiAuthResponse()
        {
            token = string.Empty;
            perms = string.Empty;
            user = new RtmApiUser();
        }
    }
}