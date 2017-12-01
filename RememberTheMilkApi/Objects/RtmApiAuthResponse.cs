using Newtonsoft.Json;

namespace RememberTheMilkApi.Objects
{
    [JsonObject("auth")]
    public class RtmApiAuthResponse
    {
        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("perms")]
        public string Permissions { get; set; }

        [JsonProperty("user")]
        public RtmApiUser User { get; set; }

        public RtmApiAuthResponse()
        {
            Token = string.Empty;
            Permissions = string.Empty;
            User = new RtmApiUser();
        }
    }
}