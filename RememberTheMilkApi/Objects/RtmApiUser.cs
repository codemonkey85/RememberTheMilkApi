using Newtonsoft.Json;

namespace RememberTheMilkApi.Objects
{
    [JsonObject("user")]
    public class RtmApiUser
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("username")]
        public string UserName { get; set; }

        [JsonProperty("fullname")]
        public string FullName { get; set; }

        public RtmApiUser()
        {
            Id = 0;
            UserName = string.Empty;
            FullName = string.Empty;
        }
    }
}