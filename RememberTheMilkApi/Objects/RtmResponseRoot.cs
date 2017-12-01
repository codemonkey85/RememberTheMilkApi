using Newtonsoft.Json;

namespace RememberTheMilkApi.Objects
{
    [JsonObject]
    public class RtmResponseRoot
    {
        [JsonProperty("rsp")]
        public RtmResponse Response { get; set; }

        public RtmResponseRoot()
        {
            Response = new RtmResponse();
        }
    }
}