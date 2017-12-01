using Newtonsoft.Json;

namespace RememberTheMilkApi.Objects
{
    [JsonObject("user")]
    public class RtmApiUser
    {
        [JsonProperty("id")]
        public int id { get; set; }

        [JsonProperty("username")]
        public string username { get; set; }

        [JsonProperty("fullname")]
        public string fullname { get; set; }

        public RtmApiUser()
        {
            id = 0;
            username = string.Empty;
            fullname = string.Empty;
        }
    }
}